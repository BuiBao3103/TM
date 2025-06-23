using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class TourSurchargeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public TourSurchargeController(AppDbContext dbContext, IMapper mapper)
        {
            _appDbContext = dbContext;
            _mapper = mapper;
        }

        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> CreateSurcharge([FromQuery] int tourId)
        {
            var tour = await _appDbContext.Tours.FindAsync(tourId);
            if (tour == null)
            {
                return Redirect("/Tour");
            }

            var viewModel = new TourSurchargeViewModel
            {
                TourId = tourId,
                TourName = tour.Name
            };

            return View(viewModel);
        }

        // POST: Tour/CreateSurcharge
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> CreateSurcharge(TourSurchargeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var surcharge = _mapper.Map<TourSurcharge>(viewModel);
                surcharge.CreatedAt = DateTime.Now;
                var identifiedTour = await _appDbContext.Tours.FindAsync(viewModel.TourId);
                
                identifiedTour.SuggestPrice = identifiedTour.SuggestPrice + surcharge.Amount;
                identifiedTour.DiscountPrice = identifiedTour.DiscountPrice + surcharge.Amount;
                _appDbContext.Add(surcharge);
                _appDbContext.Update(identifiedTour);

                _appDbContext.Add(surcharge);
                await _appDbContext.SaveChangesAsync();
                return Redirect("/Tour/Edit/" + viewModel.TourId + "#surcharge-list");
            }

            // Nếu model không hợp lệ, lấy lại tên tour để hiển thị
            var tour = await _appDbContext.Tours.FindAsync(viewModel.TourId);
            if (tour != null)
            {
                viewModel.TourName = tour.Name;
            }

            return View(viewModel);
        }

        [HttpGet("tour-surcharge/update")]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id)
        {
            try
            {
                var oldSucharge = await _appDbContext.TourSurcharges.FindAsync(id);

                if (oldSucharge == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy phụ thu với ID đã nhập.";
                    return View("UpdateSurcharge");
                }

                var suchargeViewModel = new TourSurchargeUpdateViewModel { Id = id, Name = oldSucharge.Name, Amount = oldSucharge.Amount };

                return View("UpdateSurcharge", suchargeViewModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdateSurcharge");
            }
        }

        [HttpPost("tour-surcharge/update")]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> UpdateAsync(TourSurchargeUpdateViewModel tourSurchangeUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("UpdateSurcharge", tourSurchangeUpdate);
                }

                var oldSuchange = await _appDbContext.TourSurcharges.FindAsync(tourSurchangeUpdate.Id);
                var tour = await _appDbContext.Tours.FindAsync(oldSuchange.TourId);

                if (oldSuchange == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy phụ thu với ID đã nhập.";
                    return View("UpdateSurcharge");
                }
                var priceGap = tourSurchangeUpdate.Amount - oldSuchange.Amount;

                oldSuchange.Amount = tourSurchangeUpdate.Amount;
                oldSuchange.Name = tourSurchangeUpdate.Name;
                tour.SuggestPrice = tour.SuggestPrice + priceGap;
                tour.DiscountPrice = tour.SuggestPrice + priceGap;
                _appDbContext.Update(tour);

                await _appDbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật thành công.";
                return Redirect("/Tour/Edit/" + oldSuchange.TourId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi.";
                return View("UpdateSurcharge", tourSurchangeUpdate);
            }
        }

        [HttpPost("tour-surcharge/delete")]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> Delete([FromForm] int id, [FromForm] int tourId)
        {
            try
            {
                var oldSuchange = await _appDbContext.TourSurcharges.FindAsync(id);

                if (oldSuchange == null || oldSuchange.DeleteAt != null)
                {

                    TempData["ErrorMessage"] = "Không tìm thấy phụ thu với ID đã nhập!";
                    return Redirect("/Tour");
                }
                var tour = await _appDbContext.Tours.FindAsync(tourId);
                tour.SuggestPrice = tour.SuggestPrice - oldSuchange.Amount;
                tour.DiscountPrice = tour.DiscountPrice - oldSuchange.Amount;
                oldSuchange.DeleteAt = DateTime.Now;
                _appDbContext.Update(tour);
                
                await _appDbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa thành công.";
                return Redirect($"/Tour/Edit/{oldSuchange.TourId}");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi thao tác với dữ liệu!";
                return Redirect($"/Tour/Edit/{tourId}");
            }
        }
    }
}
