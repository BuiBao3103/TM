using Microsoft.AspNetCore.Mvc.ModelBinding;
using TM.Models;

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
    }

}
