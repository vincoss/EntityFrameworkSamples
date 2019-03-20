using System.Collections.Generic;

namespace QueryTypes_Samples.Domain
{
    public class AuthorArticleCount
    {
        public string AuthorName { get; private set; }
        public int ArticleCount { get; private set; }
        //public int AuthorId { get; private set; }
        public Author Author { get; private set; }

    }
}