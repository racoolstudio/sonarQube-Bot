using Octokit;
using System.Net.Http;
using System.Net.Http.Headers;
namespace SonarBot.Services
{
    public class SonarBotService
    {
        private GitHubClient? _githubClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly int _appId;

        // constructor 
        public SonarBotService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _appId = int.Parse(configuration["GitHub:AppId"]);
            var privateKeyPath = configuration["GitHub:PrivateKeyPath"];
            if (string.IsNullOrEmpty(privateKeyPath))
                throw new ArgumentException("GitHub:PrivateKeyPath is required.");
            if (!File.Exists(privateKeyPath))
                throw new ArgumentException("GitHub:PrivateKeyPath does not exist.");
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

        }

        private async Task SetupClient()
        {
            var jwtToken = GitHubAppAuth.GenerateJwt(_configuration["GitHub:PrivateKeyPath"], _appId);

            var appClient = new GitHubClient(new Octokit.ProductHeaderValue("sonar-bot-check"))
            {
                Credentials = new Credentials(jwtToken, AuthenticationType.Bearer)
            };

            var installations = await appClient.GitHubApps.GetAllInstallationsForCurrent();
            var installationId = installations[0].Id;

            var installationToken = await appClient.GitHubApps.CreateInstallationToken(installationId);

            _githubClient = new GitHubClient(new Octokit.ProductHeaderValue("sonar-bot-check"))
            {
                Credentials = new Credentials(installationToken.Token)
            };
        }

        public async Task CreateComment(string repositoryOwner, string repositoryName, int issueNumber, string newIssues)
        {
            if (_githubClient == null)
                await SetupClient();

            await _githubClient!.Issue.Comment.Create(repositoryOwner, repositoryName, issueNumber, newIssues);
        }




    }


}
