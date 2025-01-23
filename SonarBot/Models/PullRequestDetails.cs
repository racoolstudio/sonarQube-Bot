namespace SonarBot.Models
{
    public class PullRequestDetails
    {
        public string Created_At { get; set; }
        public User User { get; set; }
        public int number { get; set; }
        public Base Base { get; set; }
    }

    public class User
    {

        public string Login { get; set; }


    }

    public class Base
    {
        public Repo repo { get; set; }

    }

    public class Repo
    {
        public string Name { get; set; }

    }
}
