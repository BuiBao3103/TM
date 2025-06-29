using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;
using TM.Services;

namespace TM.Controllers;

[RequireAuthorize("Admin", "Sale")]
public class PassengerGroupController : Controller
{
    private readonly AppDbContext _context;

    public PassengerGroupController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PassengerGroup/Create
    public async Task<IActionResult> Create(int tourId)
    {
        // Kiểm tra tour có tồn tại không
        var tour = await _context.Tours
            .Include(t => t.Location)
            .FirstOrDefaultAsync(t => t.Id == tourId);

        if (tour == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy tour";
            return RedirectToAction("Index", "Tour");
        }

        // Kiểm tra số chỗ còn trống
        if (tour.AvailableSeats <= 0)
        {
            TempData["ErrorMessage"] = "Tour đã hết chỗ";
            return RedirectToAction("Edit", "Tour", new { id = tourId });
        }

        var viewModel = new PassengerGroupCreateViewModel
        {
            TourId = tourId,
            AssignedPrice = tour.SuggestPrice,
            DiscountPrice = tour.DiscountPrice,
            HhFee = tour.HhFee,
            RepresentativeBirthDate = new DateTime(2000, 1, 1)
        };

        ViewBag.Tour = tour;
        return View(viewModel);
    }

    // POST: PassengerGroup/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PassengerGroupCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Kiểm tra tour có tồn tại không
                var tour = await _context.Tours.FindAsync(viewModel.TourId);
                if (tour == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tour";
                    return RedirectToAction("Index", "Tour");
                }

                // Kiểm tra số chỗ còn trống
                if (tour.AvailableSeats < viewModel.TotalMember)
                {
                    ModelState.AddModelError("TotalMember", $"Chỉ còn {tour.AvailableSeats} chỗ trống");
                    var tourForView = await _context.Tours
                        .Include(t => t.Location)
                        .FirstOrDefaultAsync(t => t.Id == viewModel.TourId);
                    ViewBag.Tour = tourForView;
                    return View(viewModel);
                }

                // Lấy user ID từ session
                var userId = HttpContext.Session.GetString("AuthId");
                int? createdById = null;
                int? modifiedById = null;
                if (int.TryParse(userId, out int authId))
                {
                    createdById = authId;
                    modifiedById = authId;
                }

                // Tạo người đại diện
                var representative = new Passenger
                {
                    Code = viewModel.RepresentativeCode,
                    FullName = viewModel.RepresentativeName,
                    Phone = viewModel.RepresentativePhone,
                    Email = viewModel.RepresentativeEmail,
                    DateOfBirth = DateOnly.FromDateTime(viewModel.RepresentativeBirthDate),
                    Gender = viewModel.RepresentativeGender,
                    IdentityNumber = viewModel.RepresentativeIdentityNumber,
                    Address = viewModel.RepresentativeAddress,
                    TourId = viewModel.TourId,
                    Status = viewModel.Status,
                    AssignedPrice = viewModel.AssignedPrice,
                    CustomerPaid = viewModel.CustomerPaid,
                    DiscountPrice = viewModel.DiscountPrice ?? 0,
                    HhFee = viewModel.HhFee ?? 0,
                    CreatedAt = DateTime.Now,
                    CreatedById = createdById,
                    ModifiedAt = DateTime.Now,
                    ModifiedById = modifiedById
                };

                _context.Passengers.Add(representative);
                await _context.SaveChangesAsync();

                // Tạo đoàn khách
                var passengerGroup = new PassengerGroup
                {
                    TourId = viewModel.TourId,
                    GroupName = viewModel.GroupName,
                    RepresentativeId = representative.Id,
                    TotalMember = viewModel.TotalMember,
                    Note = viewModel.Note,
                    CreatedAt = DateTime.Now,
                    CreatedById = createdById,
                    ModifiedAt = DateTime.Now,
                    ModifiedById = modifiedById
                };

                _context.PassengerGroups.Add(passengerGroup);
                await _context.SaveChangesAsync();

                // Cập nhật PassengerGroupId cho người đại diện
                representative.PassengerGroupId = passengerGroup.Id;
                _context.Passengers.Update(representative);
                await _context.SaveChangesAsync();

                // Cập nhật số chỗ trống của tour
                tour.AvailableSeats -= viewModel.TotalMember;
                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm đoàn khách thành công";
                return RedirectToAction("Edit", "Tour", new { id = viewModel.TourId, fragment = "#passengers-list" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm đoàn khách: " + ex.Message);
            }
        }

        // Nếu có lỗi, load lại tour để hiển thị
        var tourForError = await _context.Tours
            .Include(t => t.Location)
            .FirstOrDefaultAsync(t => t.Id == viewModel.TourId);
        ViewBag.Tour = tourForError;

        return View(viewModel);
    }

    private string GeneratePassengerCode()
    {
        // Tạo mã passenger ngẫu nhiên
        var random = new Random();
        var code = $"P{DateTime.Now:yyyyMMdd}{random.Next(1000, 9999)}";
        return code;
    }
} 