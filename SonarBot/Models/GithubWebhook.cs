namespace SonarBot.Models
{
    public class GithubWebhook
    {
        public string Action { get; set; }
        public Check_run check_Run { get; set; }
       
    }

    public class Check_run { 
        
        public string Conclusion { get; set; }
        public Check_Suite check_Suite { get; set; }



    }

    public class Check_Suite
    {
        public string Head_Branch { get; set; }
    }
}
