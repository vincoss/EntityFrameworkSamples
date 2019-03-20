using System;
using System.Collections.Generic;

namespace QueryTypes_Samples.Domain
{

    public class ArticleView
    {
        public string Title { get; set; }
        public Magazine Magazine { get; set; }
        public int MagazineId { get; set; }
        public DateTime PublishDate { get; set; }

    }
}
