using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Models.Entities.Projects
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public virtual ICollection<TaskEntity> Tasks { get; set; }
    }
}
