using Microsoft.EntityFrameworkCore;
using SmartHealthCompanion.Entities;

namespace SmartHealthCompanion.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        //public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<BehaviorLog> BehaviorLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AIPlan> AIPlan { get; set; }
        public DbSet<HealthRisk> HealthRisk { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        public DbSet<ChatSession> ChatSession { get; set; }

    }
}
