namespace QueryTypes_Samples.Domain
{

    public class IssueView
    {
       public int Month { get; private set; } 
       public int Year {get;private set;}
       public  Magazine Magazine { get; private set; }
    }
}