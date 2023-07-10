using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Models.Entities.Categories
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TaskEntity> Tasks { get; set; }

    }
}
