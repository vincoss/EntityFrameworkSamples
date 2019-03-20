using System.Collections.Generic;

namespace QueryTypes_Samples.Domain
{
    public class Magazine
    {
        public int MagazineId { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public List<Article> Articles { get; set; }
        //public List<ArticleView> ArticlesView{get;set;}
    }
}
