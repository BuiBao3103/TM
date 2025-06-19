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

        public void CheckAndCancelPassenger(int passengerId, int TourId)
        {
            var passenger = _context.Passengers.FirstOrDefault(p => p.Id == passengerId);
            if (passenger != null && passenger.Status == "Reserved")
            {
                passenger.Status = "Cancelation";
                //var tour = _context.Tours.Find(TourId);
                //if (tour != null) {
                _context.SaveChanges();
            }
        }
    }
}
