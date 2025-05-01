using Microsoft.EntityFrameworkCore;
using flashcard.Models;

namespace flashcard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Flashcard> Flashcards { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure the relationship between Topic and Flashcards is correctly set
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Flashcards)
                .WithOne(f=> f.Topic)
                .HasForeignKey(f => f.TopicId);
        }
    }
}
