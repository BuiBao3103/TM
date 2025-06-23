using Microsoft.AspNetCore.Mvc.ModelBinding;
using TM.Models;
using TM.Models.Entities;

namespace TM.Services
{
    public class PassengerValidatorService
    {
        private readonly AppDbContext _context;

        public PassengerValidatorService(AppDbContext context)
        {
            _context = context;
        }

        public void ValidateDuplicatePassengerFields(
            int id,
            int tourId,
            string? identityNumber,
            string? code,
            string? passportNum,
            ModelStateDictionary modelState)
        {
            bool identityNumberExists;
            if (tourId <= 0) {
                modelState.AddModelError("", "Invalid TourId.");
                return;
            }

            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            identityNumber = identityNumber?.Trim();
            code = code?.Trim();
            passportNum = passportNum?.Trim();
            var duplicates = _context.Passengers
                .Where(p => p.TourId == tourId && p.Id != id)
                .Select(p => new
                {
                    p.IdentityNumber,
                    p.Code,
                    p.PassportNum,
                    IsIdentityNumberMatch = !string.IsNullOrWhiteSpace(identityNumber) && !string.IsNullOrWhiteSpace(p.IdentityNumber) && p.IdentityNumber.ToLower() == identityNumber.ToLower(),
                    IsCodeMatch = !string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(p.Code) && p.Code.ToLower() == code.ToLower(),
                    IsPassportNumMatch = !string.IsNullOrWhiteSpace(passportNum) && !string.IsNullOrWhiteSpace(p.PassportNum) && p.PassportNum.ToLower() == passportNum.ToLower()
                })
                .FirstOrDefault(p => p.IsIdentityNumberMatch || p.IsCodeMatch || p.IsPassportNumMatch);

            if (duplicates != null)
            {
                if (duplicates.IsIdentityNumberMatch)
                {
                    modelState.AddModelError("IdentityNumber", "Số CCCD này đã tồn tại trong tour.");
                }
                if (duplicates.IsCodeMatch)
                {
                    modelState.AddModelError("Code", "Mã này đã tồn tại trong tour.");
                }
                if (duplicates.IsPassportNumMatch)
                {
                    modelState.AddModelError("PassportNum", "Số hộ chiếu này đã tồn tại trong tour.");
                }
            }

            if (duplicates != null)
            {
                if (duplicates.IsIdentityNumberMatch)
                {
                    modelState.AddModelError("IdentityNumber", "Số CCCD này đã tồn tại trong tour.");
                }
                if (duplicates.IsCodeMatch)
                {
                    modelState.AddModelError("Code", "Mã này đã tồn tại trong tour.");
                }
                if (duplicates.IsPassportNumMatch)
                {
                    modelState.AddModelError("PassportNum", "Số hộ chiếu này đã tồn tại trong tour.");
                }
            }
          
        }

        public void ValidatePassportExpiryDate(
            DateOnly? passportExpiryDate,
            Tour? tour,
            ModelStateDictionary modelState){
                if (!passportExpiryDate.HasValue) return;

                if (tour?.StartDate == null) return;

                DateOnly startDate = DateOnly.FromDateTime(tour.StartDate);
                DateOnly requiredMinExpiry = startDate.AddMonths(6);

                if (passportExpiryDate.Value < requiredMinExpiry){
                    modelState.AddModelError("PassportExpiryDate", "Hộ chiếu phải còn hạn ít nhất 6 tháng tính từ ngày khởi hành Tour.");
                }
        }
    }
}
