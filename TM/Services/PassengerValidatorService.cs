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
            bool identityNumberExists = _context.Passengers.Any(p => p.IdentityNumber == identityNumber
                                                                && p.TourId == tourId && p.Id != id);
            if (identityNumberExists)
            {
                modelState.AddModelError("IdentityNumber", "Số CCCD này đã tồn tại trong tour.");
            }

            bool codeExists = _context.Passengers.Any(p => p.Code == code && p.TourId == tourId && p.Id != id);
            if (codeExists)
            {
                modelState.AddModelError("Code", "Mã này đã tồn tại trong tour.");
            }

            bool passportExits = _context.Passengers.Any(p => p.PassportNum == passportNum && p.TourId == tourId && p.Id != id);
            if (passportExits)
            {
                modelState.AddModelError("PassportNum", "Số hộ chiếu này đã tồn tại trong tour.");
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
