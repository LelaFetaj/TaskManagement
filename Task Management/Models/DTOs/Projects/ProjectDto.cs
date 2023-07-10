using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Models.DTOs.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

    }
}
