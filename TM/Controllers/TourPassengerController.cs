using Microsoft.AspNetCore.Mvc;
using TM.Models;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class TourPassengerController : Controller
    {
        AppDbContext _appDbContext;

        public TourPassengerController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet("tour-passenger/update")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id)
        {
            try
            {
                var identifiedPassengers = await _appDbContext.Passengers.FindAsync(id);

                if (identifiedPassengers == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy khách hàng với ID đã nhập.";
                    return View("UpdatePassenger");
                }

                var passengerViewModel = new TourPassengerUpdateViewModel {
                    Id = id, 
                    FullName = identifiedPassengers.FullName, 
                    Code = identifiedPassengers.Code, 
                    DateOfBirth = identifiedPassengers.DateOfBirth,  
                    Gender = identifiedPassengers.Gender, 
                    IdentityNumber= identifiedPassengers.IdentityNumber ?? "",
                    Phone= identifiedPassengers.Phone, 
                    Email=identifiedPassengers.Email,
                    Address= identifiedPassengers.Address,
                    TourId= identifiedPassengers.TourId,
                    AssignedPrice= identifiedPassengers.AssignedPrice,
                    CustomerPaid= identifiedPassengers.AssignedPrice,
                    Status= identifiedPassengers.Status,
                    };


                return View("Update", passengerViewModel);
            } catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdateSurcharge");
            }
        }

        [HttpPost("tour-passenger/update")]
        public async Task<IActionResult> UpdateAsync(TourPassengerUpdateViewModel tourPassengerUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("UpdatePassenger", tourPassengerUpdate);
                }

                var oldPassenger = await _appDbContext.Passengers.FindAsync(tourPassengerUpdate.Id);

                if (oldPassenger == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy khách hàng với ID đã nhập.";
                    return View("UpdatePassenger");
                }


                oldPassenger.FullName = tourPassengerUpdate.FullName ?? "";
                await _appDbContext.SaveChangesAsync();

                return Redirect("/Tour/Edit/" + oldPassenger.TourId);
            } catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdatePassenger", tourPassengerUpdate);
            }
        }


    }
}
