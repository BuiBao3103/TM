namespace TM.Models.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int[] PageSizeOptions { get; set; } = [10, 20, 50, 100];
        public required string ActionName { get; set; }
    }
}
