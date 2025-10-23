namespace PDFShelf.Api.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Free, Basic, Premium
        public double StorageLimitMB { get; set; } // MB limit
        public bool CanAnnotate { get; set; } = true;
        public bool CanShare { get; set; } = false;
        public double? MonthlyPrice { get; set; } = null;
        public bool IsActive { get; set; } = true;

        public ICollection<User>? Users { get; set; }
    }
}