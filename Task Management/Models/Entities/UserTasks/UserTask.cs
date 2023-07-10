using System.Text.Json.Serialization;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.Users;

namespace Task_Management.Models.Entities.UserTasks
{
    public class UserTask
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        [JsonIgnore]
        public TaskEntity Task { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
