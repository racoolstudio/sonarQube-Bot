
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;
using SonarBot.Services;
using SonarBot.Models;
using Octokit;
using Status = SonarBot.Models.Status;

namespace SonarBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly SonarBotService _sonarBotService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _sonarToken;
        private readonly string _gitHubToken;
        private readonly string _sonarServer;
        private readonly string _githubServer;


        private readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WebhookController(ILogger<WebhookController> logger, SonarBotService sonarBotService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _sonarBotService = sonarBotService;
            _httpClientFactory = httpClientFactory;
            _sonarToken = configuration["SonarQube:Token"];
            _gitHubToken = configuration["GitHub:Token"];
            _sonarServer = configuration["SonarQube:Server"];
            _githubServer = configuration["GitHub:Server"];

        }

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonElement payload)
        {
            // Deserialize the payload to the model
            var webhookPayload = JsonSerializer.Deserialize<Status>(payload.GetRawText(), serializerOptions);
            if (webhookPayload.State == "success")
            {
                // Define the regex pattern
                string pattern = @"(?<=\/job\/)[^\/]+(?=\/[\d])";

                // Create a Regex object with the pattern
                Regex regex = new Regex(pattern);
                Match match = regex.Match(webhookPayload.Target_url);

                Regex prNumber = new Regex(@"\d+");
                Match matchPR = prNumber.Match(match.Value);

                if (match.Value.Contains("PR"))
                {
                    string pr_url = $"https://api.github.com/repos/{webhookPayload.Name}/pulls/{matchPR.Value}";

                    string sonar_analysis_issue_url;
                    PullRequestDetails prDetails;
                    SonarQubeData sonarDetails;
                    var repoOwner = "";
                    var repoName = "";
                    int issueNumber;
                    string sonar_url = "";

                    try
                    {
                        // Call method to fetch PR details
                        prDetails = await GetPRDetails(_gitHubToken, pr_url);
                        repoName = prDetails.Base.repo.Name;
                        issueNumber = prDetails.number;
                        //string sonar_url = $"http://{_sonarServer}/api/issues/search?components={repoName}_PR-{issueNumber}";
                        sonar_url = $"https://{_sonarServer}/api/issues/search?components={repoName}_{match.Value}";

                        sonarDetails = await GetSonarDetails(_sonarToken, sonar_url);
                        // You can now use prDetails (e.g., logging or further processing)
                        _logger.LogInformation($"PR User: {prDetails?.User}, PR Created At: {prDetails?.Created_At}");

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching PR details: {ex.Message}");
                        return StatusCode(500, $"Internal server error while fetching PR details.{ex.Message} ");
                    }


                    DateTime prCreateDate = DateTime.Parse(prDetails.Created_At);
                    DateTime sonarIssueCreateDate;
                    string resultOutput = "";

                    var listOfNewIssues = 0;
                    foreach (var Issue in sonarDetails.Issues)
                    {
                        sonarIssueCreateDate = DateTime.Parse(Issue.CreationDate);
                        if (sonarIssueCreateDate >= prCreateDate)
                        {
                            listOfNewIssues++;
                            resultOutput += $"Issue {listOfNewIssues} :  https://{_sonarServer}/code?id={repoName}_{match.Value}&selected={Issue.Component}\n";

                        }




                    }


                    if (listOfNewIssues == 0)
                    {
                        resultOutput = $"Your commit has no new issues(s), Nice work ! ";


                    }
                    else
                    {

                        resultOutput += $"Your commit has {listOfNewIssues} new issues(s) !";
                    }

                    await _sonarBotService.CreateComment("TownSuite", repoName, issueNumber, resultOutput);

                }


            }

            // Get the pull request URL dynamically from the payload

            return Ok();


        }

        // Method to fetch PR details
        private async Task<PullRequestDetails> GetPRDetails(string token, string prUrl)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                // Add the User-Agent header (GitHub requires this header for all requests)
                client.DefaultRequestHeaders.Add("User-Agent", "SonarQubeBot");

                // Add the Authorization header if using a token
                client.DefaultRequestHeaders.Add("Authorization", $"token {token}");

                // Make the API request to fetch PR details
                HttpResponseMessage response = await client.GetAsync(prUrl);

                // Ensure the request was successful (status code 2xx)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string content = await response.Content.ReadAsStringAsync();

                // Deserialize to the appropriate model class (define PullRequestDetails as needed)
                var prDetails = JsonSerializer.Deserialize<PullRequestDetails>(content, serializerOptions);

                return prDetails;
            }
        }


        private async Task<SonarQubeData> GetSonarDetails(string token, string Url)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {


                // Add the Authorization header if using a token
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                // Make the API request to fetch PR details
                HttpResponseMessage response = await client.GetAsync(Url);

                // Ensure the request was successful (status code 2xx)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string content = await response.Content.ReadAsStringAsync();

                // Deserialize to the appropriate model class (define PullRequestDetails as needed)
                var sonarDetails = JsonSerializer.Deserialize<SonarQubeData>(content, serializerOptions);

                return sonarDetails;
            }
        }

    }

}

