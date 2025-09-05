using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using TaskManager_api.Models;

namespace TaskManager_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectUser> ProjectUsers => Set<ProjectUser>();
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TaskTag> TaskTags => Set<TaskTag>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            // --- Table names (singular classes -> exact ERD table names) ---
            model.Entity<User>().ToTable("user");
            model.Entity<Project>().ToTable("project");
            model.Entity<ProjectUser>().ToTable("project_user");
            model.Entity<Board>().ToTable("board");
            model.Entity<BoardColumn>().ToTable("board_column");
            model.Entity<TaskItem>().ToTable("task");
            model.Entity<Comment>().ToTable("comment");
            model.Entity<Attachment>().ToTable("attachment");
            model.Entity<Tag>().ToTable("tag");
            model.Entity<TaskTag>().ToTable("task_tag");
            model.Entity<RefreshToken>().ToTable("refresh_token");


            // --- Keys ---
            model.Entity<User>().HasKey(x => x.UserId);
            model.Entity<Project>().HasKey(x => x.ProjectId);
            model.Entity<Board>().HasKey(x => x.BoardId);
            model.Entity<BoardColumn>().HasKey(x => x.ColumnId);
            model.Entity<TaskItem>().HasKey(x => x.TaskId);
            model.Entity<Comment>().HasKey(x => x.CommentId);
            model.Entity<Attachment>().HasKey(x => x.AttachmentId);
            model.Entity<Tag>().HasKey(x => x.TagId);
            model.Entity<RefreshToken>().HasKey(x=>x.Id);

            // Composite keys
            model.Entity<ProjectUser>().HasKey(x => new { x.ProjectId, x.UserId });
            model.Entity<TaskTag>().HasKey(x => new { x.TaskId, x.TagId });

            // --- Relationships ---
                model.Entity<RefreshToken>()
             .HasOne(rt => rt.User)
             .WithMany(u => u.RefreshTokens)
             .HasForeignKey(rt => rt.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Project>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<Board>()
                .HasOne(b => b.Project)
                .WithMany(p => p.Boards)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<BoardColumn>()
                .HasOne(c => c.Board)
                .WithMany(b => b.Columns)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<TaskItem>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<TaskItem>()
                .HasOne(t => t.Column)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.NoAction);

            model.Entity<TaskItem>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.TasksAssigned)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.SetNull);

            model.Entity<TaskItem>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.TasksCreated)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            model.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Attachment>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Attachment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attachments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Tag>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tags)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Unique & length rules ---
            model.Entity<User>().HasIndex(u => u.Email).IsUnique();


            // --- Set default value -----
            model.Entity<Project>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
            model.Entity<ProjectUser>()
                .Property(pu => pu.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
            // --- (Optional) Tự động snake_case cột/constraint/index ---
            ToSnakeCaseAll(model);
        }

        // Convert toàn bộ tên cột/index/constraint sang snake_case
        private static void ToSnakeCaseAll(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // table
                var tableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(tableName))
                    entity.SetTableName(ToSnakeCase(tableName));

                // columns
                foreach (var property in entity.GetProperties())
                    property.SetColumnName(ToSnakeCase(property.Name));

                // keys
                foreach (var key in entity.GetKeys())
                    key.SetName(ToSnakeCase(key.GetName()));

                // foreign keys
                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));

                // indexes
                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }

        private static string ToSnakeCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            var s = Regex.Replace(name, "([a-z0-9])([A-Z])", "$1_$2");
            s = Regex.Replace(s, "([A-Z])([A-Z][a-z])", "$1_$2");
            return s.ToLowerInvariant();
        }
    }
}
