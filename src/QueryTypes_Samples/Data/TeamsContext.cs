using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using QueryTypes_Samples.Domain;
using System.Linq;

namespace QueryTypes_Samples.Data
{
    public class PublicationsContext : DbContext
    {
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbQuery<AuthorArticleCount> AuthorArticleCounts { get; set; }
        public DbSet<Author> Authors { get; set; }

        // public DbQuery<ArticleView> ArticleView { get; set; }
        // public DbQuery<Publisher> Publishers{get;set;}
        // public DbQuery<MagazineStatsView> MagazineStats{get;set;}

       // public static readonly LoggerFactory MyConsoleLoggerFactory
       // = new LoggerFactory(new[] 
       //{
       //       new ConsoleLoggerProvider((category, level)
       //         => category == DbLoggerCategory.Database.Command.Name
       //        && level == LogLevel.Information, true) });

        private ILoggerFactory CreateLoggerFactory()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddFilter("QueryTypes_Samples.Data.Program", LogLevel.Debug)
                       .AddConsole();
            });

            return loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = CreateLoggerFactory();
            optionsBuilder.UseLoggerFactory(loggerFactory)
           .UseSqlite(@"Filename=Data/PubsTracker.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>();
            modelBuilder.Entity<ArticleView>().HasOne(a => a.Magazine).WithMany();
            modelBuilder.Entity<IssueView>().HasOne(i => i.Magazine).WithMany();
            modelBuilder.Entity<Author>().Property(p => p.Name).HasColumnName("AuthorName");
            modelBuilder.Entity<AuthorArticleCount>().ToView("View_AuthorArticleCounts");
            modelBuilder.Entity<AuthorArticleCount>().HasOne(a => a.Author).WithOne(); //.HasForeignKey<AuthorArticleCount>(a => a.AuthorId);
            modelBuilder.Entity<MagazineStatsView>().ToQuery(
                () => Magazines.Select(m => new MagazineStatsView(
                             m.Name,
                             m.Articles.Count,
                             m.Articles.Select(a => a.AuthorId).Distinct().Count()
                            )
                          )
            );

            modelBuilder.Entity<Magazine>().HasData(new Magazine { MagazineId = 1, Name = "MSDN Magazine" });
            modelBuilder.Entity<Article>().HasData(
              new Article { ArticleId = 1, MagazineId = 1, Title = "EF Core 2.1 Query Types", AuthorId = 1 },
              new Article { ArticleId = 2, MagazineId = 1, Title = "Creating Azure Functions That Can Read from Cosmos DB with Almost No Code", AuthorId = 1 }
             );
            modelBuilder.Entity<Magazine>().HasData(new Magazine { MagazineId = 2, Name = "New Yorker" });
            modelBuilder.Entity<Article>().HasData(
              new Article { ArticleId = 3, MagazineId = 2, Title = "Reddit and the Struggle to Detoxify the Internet", AuthorId = 2 },
              new Article { ArticleId = 4, MagazineId = 2, Title = "Digital Vigilantes", AuthorId = 3 }
            );
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "Julie Lerman" },
                new Author { AuthorId = 2, Name = "Andrew Marantz" },
                new Author { AuthorId = 3, Name = "Nicholas Schmidle" }

            );
           
        }
    }
}