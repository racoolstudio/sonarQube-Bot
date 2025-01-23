<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var password33="passworfdfe";
=======
=======
>>>>>>> Stashed changes
﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    // Ensure that your Main method is async
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string token = "sqa_1c96a3a006f9a8cf1ad26a9376b06669112e5b32"; // Your GitHub token

        using (HttpClient client = new HttpClient())
        {
            // Add the User-Agent header (GitHub requires this header for all requests)

            // Add the Authorization header if using a token
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            try
            {
                // Make the API request to fetch PR details
                HttpResponseMessage response = await client.GetAsync("http://172.105.20.48/api/issues/search?components=DummyProject_main");

                // Ensure the request was successful (status code 2xx)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string content = await response.Content.ReadAsStringAsync();

                // Log the response content to understand what GitHub is returning
                Console.WriteLine("Response content: ");
                Console.WriteLine(content);

                // Deserialize the response content to a model (optional, based on your needs)
                // var prDetails = JsonSerializer.Deserialize<PullRequestDetails>(content, serializerOptions);
                // Console.WriteLine($"PR Title: {prDetails?.Title}, PR URL: {prDetails?.Url}");

            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP request errors
                Console.WriteLine("HTTP Request Error: " + httpEx.Message);
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.WriteLine("Error fetching PR details: " + ex.Message);
            }

        }
       
    }
}
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
