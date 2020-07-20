using System.Data.Entity;


namespace TaskMasterTut.Model
{
    class tmDBcontext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Task> Tasks { get; set; }

    }
}
