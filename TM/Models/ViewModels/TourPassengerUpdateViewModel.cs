
namespace TM.Models.ViewModels
{
    public class TourPassengerUpdateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string IdentityNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int TourId { get; set; }
        public decimal AssignedPrice { get; set; }
        public decimal CustomerPaid { get; set; }
        public string Status { get; set; }
        public string TourName { get; set; }
    }
}
