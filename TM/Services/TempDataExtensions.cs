using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace TM.Services
{
    public static class TempDataExtensions
    {
        private const string Key = "ModelStateErrors";

        // CẢNH BÁO: KHÔNG sử dụng TempData cho object phức tạp (JArray, JObject, List, Dictionary, ...)!
        // Nên chỉ dùng TempData cho string, int, bool hoặc object đã serialize thành string.
        // Nếu cần truyền dữ liệu phức tạp, hãy dùng query string hoặc ViewBag.
        public static void PutModelState(this ITempDataDictionary tempData, ModelStateDictionary modelState)
        {
            var errors = modelState.Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            tempData[Key] = JsonConvert.SerializeObject(errors);
        }

        public static void RestoreModelState(this Controller controller)
        {
            if (controller.TempData.TryGetValue(Key, out var temp) && temp is string json)
            {
                var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
                foreach (var error in errors)
                {
                    foreach (var message in error.Value)
                    {
                        controller.ModelState.AddModelError(error.Key, message);
                    }
                }

                controller.TempData.Remove(Key);
            }
        }
    }
}
