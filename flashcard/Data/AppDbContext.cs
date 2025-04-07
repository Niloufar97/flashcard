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
                .WithOne(f => f.Topic)
                .HasForeignKey(f => f.TopicId);

            // Seed initial topic
            modelBuilder.Entity<Topic>().HasData(new Topic
            {
                TopicId = 1,  // Static Id for the topic
                Name = "Food",
            });

            // Seed 10 flashcards for "Food" topic
            modelBuilder.Entity<Flashcard>().HasData(
                new Flashcard { Id = 1, TopicId = 1, DanishWord = "brød", EnglishWord = "bread" },
                new Flashcard { Id = 2, TopicId = 1, DanishWord = "ost", EnglishWord = "cheese" },
                new Flashcard { Id = 3, TopicId = 1, DanishWord = "mælk", EnglishWord = "milk" },
                new Flashcard { Id = 4, TopicId = 1, DanishWord = "æble", EnglishWord = "apple" },
                new Flashcard { Id = 5, TopicId = 1, DanishWord = "banan", EnglishWord = "banana" },
                new Flashcard { Id = 6, TopicId = 1, DanishWord = "kød", EnglishWord = "meat" },
                new Flashcard { Id = 7, TopicId = 1, DanishWord = "fisk", EnglishWord = "fish" },
                new Flashcard { Id = 8, TopicId = 1, DanishWord = "vand", EnglishWord = "water" },
                new Flashcard { Id = 9, TopicId = 1, DanishWord = "salt", EnglishWord = "salt" },
                new Flashcard { Id = 10, TopicId = 1, DanishWord = "smør", EnglishWord = "butter" }
            );
        }
    }
}
