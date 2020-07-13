using Microsoft.EntityFrameworkCore;
using QueryTypes_Samples.Data;
using QueryTypes_Samples.Domain;
using System;
using System.Linq;

namespace QueryTypes_Samples
{
    class Program
    {
        private readonly static PublicationsContext _context = new PublicationsContext();

        static void Main(string[] args)
        {
            //QueryAuthorArticleCountView();
            //QueryOneToOne();
            //QueryArticleCountViewWithArticle();
            // QueryIssuesWithMags();

            //MagazineStats();
            //QueryTypeFromSql();
            //PublisherQuery();
            //QueryOneToManyView();

            Console.WriteLine("Done...");
            Console.Read();
        }

        private static void QueryOneToManyView()
        {
            var articles = _context.Set<ArticleView>().Include(m => m.Magazine).ToList();

            foreach(var article in articles)
            {
                Console.WriteLine($"{article.Title}");
            }
        }

        private static void QueryIssuesWithMags()
        {
            //var results = _context.Query<IssueView>().Include(i => i.Magazine).ToList();

            //foreach (var result in results)
            //{
            //    Console.WriteLine($"{result.Year}");
            //}
        }

        private static void QueryArticleCountViewWithArticle()
        {
            var results = _context.Set<AuthorArticleCount>().Include(a => a.Author).ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.AuthorName}");
            }
        }

        private static void PublisherQuery()
        {
            var results = _context.Set<Publisher>().FromSqlRaw("select name, yearfounded from publishers").ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name}");
            }
        }

        private static void QueryTypeFromSql()
        {
            var results = _context.Authors.FromSqlRaw("select authorid,authorname from authors").ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name}");
            }
        }

        private static void MagazineStats()
        {
            // var results=_context.Magazines
            // .Select(m=>new{m.Name,
            //                TheCount=m.Articles.Count,
            //                uniqueauthors=m.Articles.Select(a=>a.AuthorId).Distinct().Count()
            //                }
            // ).ToList();

            var results = _context.Set<MagazineStatsView>().ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name}, {result.ArticleCount}, count: {result.AuthorCount}");
            }
        }

        private static void QueryAuthorArticleCountView()
        {
            //var results = _context.AuthorArticleCounts.ToList();
            var results = _context.Set<AuthorArticleCount>().ToList();

            foreach(var result in results)
            {
                Console.WriteLine($"{result.AuthorName}, count: {result.ArticleCount}");
            }
        }

        private static void QueryOneToOne()
        {
            var results = _context.AuthorArticleCounts.Include(a => a.Author).ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.AuthorName}, {result.Author.Name}, count: {result.ArticleCount}");
            }
        }
    }
}
