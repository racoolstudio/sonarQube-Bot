# SonarQube-Bot

**SonarQube-Bot** is a tool that integrates SonarQube with GitHub to automatically create comments on pull requests based on the analysis results. When a new commit is pushed to a pull request, the bot reviews the issues reported by SonarQube and creates a comment highlighting the new issues identified in that commit.

## Features
- **Automatic Issue Reporting**: SonarQube-Bot automatically fetches issues from SonarQube related to the current commit and creates comments on GitHub pull requests.
- **Integration with Jenkins**: Works with Jenkins for continuous integration.
- **Uses GitHub and SonarQube Tokens**: The bot uses tokens from GitHub and SonarQube for authentication and authorization.

## Prerequisites

1. **SonarQube Instance**:
   - A running SonarQube server where your project is analyzed.
   - SonarQube token to authenticate API requests.

2. **GitHub Repository**:
   - A GitHub repository where you want to enable the SonarQube-Bot.
   - GitHub token to authenticate API requests.

3. **Jenkins**:
   - Jenkins setup for code analysis and CI/CD pipeline that triggers SonarQube analysis.

4. **Webhook Configuration**:
   - You need to set up a webhook on GitHub to send data to the bot whenever a commit is pushed to a pull request.

---

## Setup Instructions

### Step 1: Configure the Webhook in GitHub
1. Go to your GitHub repository's settings.
2. Navigate to **Webhooks** and click **Add webhook**.
3. Set the **Payload URL** to the URL where your SonarQube-Bot is hosted (e.g., `https://yourdomain.com/api/webhook`).
4. Set the **Content type** to `application/json`.
5. Select the events that will trigger the webhook. Choose **check_suite**, **check**, **status** and **Push** events to capture commit details.
6. Ensure the webhook sends the `check_suite` and `status` events. This ensures that the bot can track the build status.

### Step 2: Configure SonarQube and GitHub Tokens
1. **SonarQube Token**: In your SonarQube instance, generate a personal access token by going to **My Account** > **Security** and clicking on **Generate Tokens**. Copy this token.
2. **GitHub Token**: Generate a GitHub personal access token with the necessary permissions (e.g., `repo`, `pull_requests`). Go to your GitHub account settings > **Developer settings** > **Personal access tokens** and create a new token. Copy this token.

### Step 3: Set Configuration in the Application
In the configuration file (e.g., `appsettings.json` or environment variables), set the following values:

```json
{
  "SonarQube": {
    "Token": "YOUR_SONARQUBE_TOKEN",
    "Server": "https://your-sonarqube-server.com"
  },
  "GitHub": {
    "Token": "YOUR_GITHUB_TOKEN",
    "Server": "racoolstudio"
  }
}
```
