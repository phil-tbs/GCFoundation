namespace Foundation.Web.Models
{
    public class FoundingOpportunity
    {
        public required Guid Id { get; set; }

        public required string Section { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }

        public double Allocation {  get; set; }

        public FoundingOpportunityStatus Status { get; set; }

        public DateOnly StatusDate { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }

    public enum FoundingOpportunityStatus
    {
        Open,
        Close
    }
}
