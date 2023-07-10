using System.Text.Json.Serialization;
using Task_Management.Models.Entities.Categories;
using Task_Management.Models.Entities.Projects;
using Task_Management.Models.Entities.Users;
using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Models.Entities.Tasks
{
    public class TaskEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; } 

        public Urgency Urgency { get; set; }

        public Guid? CategoryId { get; set; } //Category table foregin key

        public Guid? ProjectId { get; set; } //Project table ferign key

        public Guid? AssignedUserId { get; set; }

        public int Progress { get; set; }


        [JsonIgnore]
        public Project Project { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<UserTask> UserTasks { get; set; }

    }
}
