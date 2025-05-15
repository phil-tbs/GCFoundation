namespace Foundation.Web.Models
{
    public class FoundingOpportunity
    {
        public Guid Id { get; set; }

        public string Section { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public double Allocation {  get; set; }

        public FoundingOpportunityStatus Status { get; set; }

        public DateOnly StatusDate { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    public enum FoundingOpportunityStatus
    {
        Open,
        Close
    }
}
