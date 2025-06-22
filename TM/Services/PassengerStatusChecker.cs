using TM.Enum;
using TM.Models;
using TM.Models.Entities;

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

                var tour = _context.Tours.Find(passenger.TourId);
                if (tour != null)
                {
                    int bookedSeatsAmount = _context.Passengers
                        .Where(p => p.TourId == tour.Id && p.Status != PassengerStatus.Cancelled.ToString())
                        .Count();

                    tour.AvailableSeats = tour.TotalSeats - bookedSeatsAmount;
                    _context.SaveChanges();
                }
            }
        }

        public void CheckFullpayDeadline(int tourId)
        {
            var tour = _context.Tours.Find(tourId);
            if (tour == null)
                return;

            var passengers = _context.Passengers
                .Where(p => p.TourId == tourId && p.Status == PassengerStatus.Confirmed.ToString())
                .ToList();

            if (passengers.Count == 0)
                return;

            foreach (var passenger in passengers)
            {
                passenger.Status = PassengerStatus.Cancelled.ToString();
            }

            _context.SaveChanges(); 

            int bookedSeatsAmount = _context.Passengers
                .Where(p => p.TourId == tour.Id && p.Status != PassengerStatus.Cancelled.ToString())
                .Count();

            tour.AvailableSeats = tour.TotalSeats - bookedSeatsAmount;
            _context.SaveChanges();
        }
    }
}
