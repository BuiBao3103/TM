using TM.Enum;
using TM.Models;

namespace TM.Services
{
    public class PassengerStatusChecker
    {
        private readonly AppDbContext _context;
        public PassengerStatusChecker(AppDbContext context)
        {
            _context = context;
        }

        public void CheckHoldTime(int passengerId)
        {
            var passenger = _context.Passengers.Find(passengerId);
            if (passenger != null && passenger.Status == PassengerStatus.Reserved.ToString())
            {
                passenger.Status = PassengerStatus.Cancelled.ToString();
                _context.SaveChanges();
            }
        }

        public void CheckFullpayDeadline(int tourId)
        {
            var passengers = _context.Passengers
                .Where(p => p.TourId == tourId && p.Status == PassengerStatus.Confirmed.ToString())
                .ToList();

            foreach (var passenger in passengers)
            {
                passenger.Status = PassengerStatus.Cancelled.ToString();
            }

            if (passengers.Count != 0)
            {
                _context.SaveChanges();
            }
        }

    }
}
