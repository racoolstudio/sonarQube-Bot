namespace SonarBot.Models
{
    public class SonarQubeData
    {
        public int Total { get; set; }
        public List<Issues> Issues { get; set; }
    }


    public class Issues
    {
        public string Component { get; set; }
        public string Author { get; set; }
        public string CreationDate { get; set; }

    }

}
