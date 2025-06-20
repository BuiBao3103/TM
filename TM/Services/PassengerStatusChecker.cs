using TM.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var passenger =  _context.Passengers.Find(passengerId);
            if (passenger != null && passenger.Status == "Reserved")
            {
                passenger.Status = "Cancelled";
                _context.SaveChanges();
            }
        }

        public void CheckFullpayDeadline(int tourId)
        {
            var passengers = _context.Passengers
                .Where(p => p.TourId == tourId && p.Status == "Confirmed")
                .ToList();

            foreach (var passenger in passengers)
            {
                passenger.Status = "Cancelled";
            }

            if (passengers.Count != 0)
            {
                _context.SaveChanges();
            }
        }

    }
}
