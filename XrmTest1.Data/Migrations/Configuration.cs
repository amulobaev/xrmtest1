using System.Data.Entity.Migrations;

namespace XrmTest1.Data.Migrations
{
    /// <summary>
    /// Конфигурация миграций
    /// </summary>
    internal class Configuration : DbMigrationsConfiguration<ResumeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}