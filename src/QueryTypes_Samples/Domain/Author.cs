using System.Collections.Generic;

namespace QueryTypes_Samples.Domain
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<Article> Articles { get; set; }
       }
}