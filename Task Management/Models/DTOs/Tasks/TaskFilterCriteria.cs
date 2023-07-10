using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Models.DTOs.Tasks
{
    public class TaskFilterCriteria
    {
        public string Keyword { get; set; }
        public Guid? AssignedUserId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public Status? Status { get; set; }
        public Priority? Priority { get; set; }
        public Urgency? Urgency { get; set; }
    }
}
