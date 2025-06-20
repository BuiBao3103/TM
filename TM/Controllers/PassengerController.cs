using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TM.Enum;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class PassengerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PassengerController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("passenger/filter")]
        public IActionResult Filter([FromBody] PassengerFilterViewModel viewModel)
        {

            var query = _context.Passengers
                .Select(p => new Passenger
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Phone = p.Phone,
                    AssignedPrice = p.AssignedPrice,
                    CustomerPaid = p.CustomerPaid,
                    PassportNum = p.PassportNum,
                    DepartureFlightInfo = p.DepartureFlightInfo,
                    ArrivalFlightInfo = p.ArrivalFlightInfo,
                    Status = p.Status,
                })
                .Where(p => p.DeleteAt == null && p.TourId == viewModel.TourId)
                .AsQueryable();

            // check keyword
            if (!string.IsNullOrWhiteSpace(viewModel.Keyword))
            {
                query = query.Where(p =>
                    p.FullName.Contains(viewModel.Keyword) ||
                    (p.IdentityNumber != null && p.IdentityNumber.Contains(viewModel.Keyword)) ||
                    (p.Phone != null && p.Phone.Contains(viewModel.Keyword)));
            }

            // check status
            if (!string.IsNullOrWhiteSpace(viewModel.Status))
            {
                query = query.Where(p => p.Status == viewModel.Status);
            }

            // check group
            if (viewModel.PassengerGroup.HasValue)
            {
                if (viewModel.PassengerGroup == PassengerGroup.Go)
                {
                    query = query.OrderBy(p => p.DepartureFlightInfo);
                }
                else
                {
                    query = query.OrderBy(p => p.ArrivalFlightInfo);
                }
            }

            var listPassenger = query.ToList();

            return View();
        }
    }
}
