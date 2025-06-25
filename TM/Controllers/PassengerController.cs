using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [RequireAuthorize("Admin", "Sale")]
        [HttpGet("passenger/filter")]
        public async Task<IActionResult> Filter([FromQuery] PassengerFilterViewModel viewModel)
        {
            try
            {
                var query = _context.Passengers
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
                if (viewModel.Status != null)
                {
                    query = query.Where(p => viewModel.Status.Contains(p.Status));
                }

                var listPassenger = await query.Select(p => new Passenger
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Phone = p.Phone,
                    AssignedPrice = p.AssignedPrice,
                    CustomerPaid = p.CustomerPaid,
                    PassportNum = p.PassportNum,
                    PassportExpiryDate = p.PassportExpiryDate,
                    DepartureFlightInfo = p.DepartureFlightInfo,
                    ArrivalFlightInfo = p.ArrivalFlightInfo,
                    Status = p.Status,
                    ModifiedById = p.ModifiedById,
                    HhFee = p.HhFee,
                    DiscountPrice = p.DiscountPrice,
                }).ToListAsync();

                var listPassengerViewModel = _mapper.Map<List<PassengerViewModel>>(listPassenger);

                if (viewModel.PassengerGroup != null)
                {
                    ViewData["GroupBy"] = viewModel.PassengerGroup;
                    return PartialView("~/Views/Passenger/Shared/_TablePassengerGroupPartial.cshtml", listPassengerViewModel);
                }

                return PartialView("~/Views/Passenger/Shared/_TablePassengerGroupPartial.cshtml", listPassengerViewModel);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Passenger/Shared/_ErrorSearchPartial.cshtml", "Lỗi rồi, bọ siêu khổng lồ!");
            }
        }

        [RequireAuthorize("Admin", "Sale")]
        [HttpGet("passenger/quick-filter")]
        public async Task<IActionResult> QuickFilterFromListTour(
            [FromQuery] string? PassengerGroup,
            [FromQuery] string[]? status,
            [FromQuery] string tourId
        )
        {
            try
            {
                var routeValues = new RouteValueDictionary
                {
                    { "id", tourId },
                    { "PassengerGroup", PassengerGroup }
                };

                if (status != null && status.Length > 0)
                {
                    routeValues["status"] = status;
                }

                // Kiểm tra trạng thái tour
                if (int.TryParse(tourId, out int tourIdInt))
                {
                    var tour = await _context.Tours.FindAsync(tourIdInt);
                    if (tour != null && (tour.Status == "Completed" || tour.Status == "Cancelled"))
                    {
                        return RedirectToAction("Details", "Tour", routeValues);
                    }
                }

                // Nếu tour chưa hoàn thành hoặc chưa hủy, chuyển đến trang Edit
                return RedirectToAction("Edit", "Tour", routeValues);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi ở đây nếu cần
                return Redirect("/");
            }
        }
    }
}
