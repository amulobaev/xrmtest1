using System.Data.Entity;
using XrmTest1.Data.Migrations;

namespace XrmTest1.Data
{
    /// <summary>
    /// Контекст Entity Framework
    /// </summary>
    internal class ResumeContext : DbContext
    {
        public ResumeContext()
            : base("name=ResumeContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ResumeContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ResumeEntity> ResumeEntities { get; set; }
    }
}
