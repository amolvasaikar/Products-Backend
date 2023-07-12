namespace MediSearch.Domain.Common
{
    public interface IAuditableEntity
    {
        public DateTime? DateTimeCreated { get; set; }

        public DateTime? DateTimeUpdated { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
