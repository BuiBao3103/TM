using Microsoft.AspNetCore.Mvc;
using TM.Models;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class TourSurchargeController : Controller
    {
        AppDbContext _appDbContext;

        public TourSurchargeController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet("tour-surcharge/update")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id)
        {
            try
            {
                var oldSucharge = await _appDbContext.TourSurcharges.FindAsync(id);

                if (oldSucharge == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy phụ thu với ID đã nhập.";
                    return View("UpdateSurcharge");
                }

                var suchargeViewModel = new TourSurchargeUpdateViewModel { Id = id, Name = oldSucharge.Name, Amount = oldSucharge.Amount };

                return View("UpdateSurcharge", suchargeViewModel);
            } catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdateSurcharge");
            }
        }

        [HttpPost("tour-surcharge/update")]
        public async Task<IActionResult> UpdateAsync(TourSurchargeUpdateViewModel tourSurchangeUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("UpdateSurcharge", tourSurchangeUpdate);
                }

                var oldSuchange = await _appDbContext.TourSurcharges.FindAsync(tourSurchangeUpdate.Id);

                if (oldSuchange == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy phụ thu với ID đã nhập.";
                    return View("UpdateSurcharge");
                }

                oldSuchange.Amount = tourSurchangeUpdate.Amount;
                oldSuchange.Name = tourSurchangeUpdate.Name;
                await _appDbContext.SaveChangesAsync();

                return Redirect("/Tour/Edit/" + oldSuchange.TourId);
            } catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdateSurcharge", tourSurchangeUpdate);
            }
        }

        [HttpPost("tour-surcharge/delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            try
            {
                var oldSuchange = await _appDbContext.TourSurcharges.FindAsync(id);

                if (oldSuchange == null || oldSuchange.DeleteAt != null)
                {

                    ViewBag.ErrorMessage = "Không tìm thấy phụ thu với ID đã nhập.";
                    return Redirect("/Tour");
                }

                oldSuchange.DeleteAt = DateTime.Now;
                await _appDbContext.SaveChangesAsync();

                return Redirect("/Tour/Edit/" + oldSuchange.TourId);
            } catch (Exception)
            {
                return Redirect("/Tour");
            }
        }
    }
}
