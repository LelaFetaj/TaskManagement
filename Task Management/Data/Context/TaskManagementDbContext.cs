using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Management.Models.Entities.Categories;
using Task_Management.Models.Entities.Projects;
using Task_Management.Models.Entities.Roles;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.Users;
using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Data.Context
{
    public class TaskManagementDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly IConfiguration _configuration;

        public TaskManagementDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SetTableNames(builder);
            SetUserTaskReferences(builder);
            SetProjectReferences(builder);
            SetCategoryReferences(builder);
            SetUserReferences(builder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this._configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);
        }

        private static void SetTableNames(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable(name: "Roles");
            modelBuilder.Entity<User>().ToTable(name: "Users");
            modelBuilder.Entity<UserTask>().ToTable(name: "UserTasks");
            modelBuilder.Entity<TaskEntity>().ToTable(name: "Tasks");
            modelBuilder.Entity<Category>().ToTable(name: "Categories");
            modelBuilder.Entity<Project>().ToTable(name: "Projects");
        }

        private static void SetUserTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
                .HasKey(ut =>
                new { ut.UserId, ut.TaskId });

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(tasks => tasks.UserTasks)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void SetProjectReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(tk => tk.Project)
                .WithMany(projects => projects.Tasks)
                .HasForeignKey(tk => tk.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void SetCategoryReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void SetUserReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(x => x.User)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.AssignedUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<TaskEntity> TaskEntity { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserTask> UserTasks { get; internal set; }
    }
}
