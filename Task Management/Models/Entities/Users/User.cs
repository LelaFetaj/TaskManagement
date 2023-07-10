using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Models.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<UserTask> UserTasks { get; set; }

        [JsonIgnore]
        public ICollection<TaskEntity> Tasks { get; set; }
    }

}
