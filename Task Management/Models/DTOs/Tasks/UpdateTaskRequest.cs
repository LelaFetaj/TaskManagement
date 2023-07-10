using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Models.DTOs.Tasks
{
    public class UpdateTaskRequest
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public Status? Status { get; set; }

        public Priority? Priority { get; set; }

        public Urgency? Urgency { get; set; }

        public Guid? CategoryId { get; set; } //Category table foregin key

        public Guid? ProjectId { get; set; } //Project table ferign key

        public Guid? AssignedUserId { get; set; }

        public int? Progress { get; set; }
    }
}
