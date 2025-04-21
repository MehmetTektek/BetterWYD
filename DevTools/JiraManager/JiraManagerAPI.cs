using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BetterWYD.DevTools.JiraManager
{
    /// <summary>
    /// JiraManagerAPI - A C# API hook for Unity to manage Jira tasks
    /// 
    /// This API provides integration with Jira for the BetterWYD project using
    /// the Jira REST API v3 with API token authentication. It enables Unity
    /// components to directly interact with Jira for task management.
    /// 
    /// Features:
    /// - Fetch issues from Jira project
    /// - Create new issues
    /// - Update existing issues
    /// - Add comments to issues
    /// - Transition issues between statuses
    /// - Generate reports on project progress
    /// 
    /// Created as part of the BetterWYD project infrastructure on April 21, 2025
    /// </summary>
    public class JiraManagerAPI
    {
        private readonly string _jiraEmail;
        private readonly string _apiToken;
        private readonly string _jiraUrl;
        private readonly string _projectKey;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// JiraManagerAPI Constructor
        /// </summary>
        /// <param name="jiraEmail">Jira account email</param>
        /// <param name="apiToken">Jira API token</param>
        /// <param name="jiraUrl">Jira instance URL</param>
        /// <param name="projectKey">Jira project key</param>
        public JiraManagerAPI(string jiraEmail, string apiToken, string jiraUrl, string projectKey)
        {
            _jiraEmail = jiraEmail;
            _apiToken = apiToken;
            _jiraUrl = jiraUrl;
            _projectKey = projectKey;

            // Initialize HttpClient
            _httpClient = new HttpClient();
            
            // Set up authentication
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_jiraEmail}:{_apiToken}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Test the connection to the Jira API
        /// </summary>
        /// <returns>True if connection is successful, otherwise false</returns>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_jiraUrl}/rest/api/3/myself");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<JObject>(content);
                    Console.WriteLine($"Successfully connected to Jira as {userData?["displayName"] ?? "Unknown User"}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to connect to Jira: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to Jira: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get issues from the Jira project
        /// </summary>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <returns>A list of issue objects</returns>
        public async Task<List<JObject>> GetProjectIssuesAsync(int maxResults = 50)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/search";
                var jqlQuery = $"project = {_projectKey} ORDER BY created DESC";
                
                var payload = new
                {
                    jql = jqlQuery,
                    maxResults = maxResults,
                    fields = new[] { "summary", "description", "status", "assignee", "priority", "issuetype", "created", "updated" }
                };
                
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(responseContent);
                    var issues = data?["issues"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                    return issues;
                }
                else
                {
                    Console.WriteLine($"Error fetching issues: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return new List<JObject>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when fetching issues: {ex.Message}");
                return new List<JObject>();
            }
        }

        /// <summary>
        /// Create a new issue in the Jira project
        /// </summary>
        /// <param name="summary">Issue summary/title</param>
        /// <param name="description">Detailed description</param>
        /// <param name="issueType">Type of issue (e.g., "Task", "Bug")</param>
        /// <param name="parentKey">Parent issue key (required for sub-tasks)</param>
        /// <param name="priority">Priority level (e.g., "High", "Medium")</param>
        /// <param name="assigneeAccountId">Account ID of assignee</param>
        /// <returns>Created issue data or null if failed</returns>
        public async Task<JObject> CreateIssueAsync(
            string summary, 
            string description, 
            string issueType = null, 
            string parentKey = null, 
            string priority = null, 
            string assigneeAccountId = null)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issue";
                
                // Convert description to Jira's ADFV3 format
                var descriptionAdf = new JObject
                {
                    ["type"] = "doc",
                    ["version"] = 1,
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "paragraph",
                            ["content"] = new JArray
                            {
                                new JObject
                                {
                                    ["type"] = "text",
                                    ["text"] = description
                                }
                            }
                        }
                    }
                };
                
                // Prepare the payload
                var fields = new JObject
                {
                    ["project"] = new JObject { ["key"] = _projectKey },
                    ["summary"] = summary,
                    ["description"] = descriptionAdf
                };
                
                // Add issue type if provided
                if (!string.IsNullOrEmpty(issueType))
                {
                    fields["issuetype"] = new JObject { ["name"] = issueType };
                    
                    // If parent key is provided and this is a sub-task
                    if (!string.IsNullOrEmpty(parentKey))
                    {
                        fields["parent"] = new JObject { ["key"] = parentKey };
                    }
                }
                
                // Add priority if provided
                if (!string.IsNullOrEmpty(priority))
                {
                    fields["priority"] = new JObject { ["name"] = priority };
                }
                
                // Add assignee if provided
                if (!string.IsNullOrEmpty(assigneeAccountId))
                {
                    fields["assignee"] = new JObject { ["id"] = assigneeAccountId };
                }
                
                var payload = new JObject
                {
                    ["fields"] = fields
                };
                
                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(responseContent);
                    Console.WriteLine($"Successfully created issue: {data?["key"]}");
                    return data;
                }
                else
                {
                    Console.WriteLine($"Error creating issue: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when creating issue: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update an existing Jira issue
        /// </summary>
        /// <param name="issueKey">The key of the issue to update (e.g., 'BWYD-123')</param>
        /// <param name="fieldsToUpdate">Dictionary of field keys and values to update</param>
        /// <returns>True if successful, False otherwise</returns>
        public async Task<bool> UpdateIssueAsync(string issueKey, Dictionary<string, object> fieldsToUpdate)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issue/{issueKey}";
                
                // Build the payload based on provided fields
                var fields = new JObject();
                
                foreach (var field in fieldsToUpdate)
                {
                    if (field.Value is string stringValue)
                    {
                        fields[field.Key] = stringValue;
                    }
                    else
                    {
                        fields[field.Key] = JToken.FromObject(field.Value);
                    }
                }
                
                var payload = new JObject
                {
                    ["fields"] = fields
                };
                
                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully updated issue: {issueKey}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error updating issue: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when updating issue: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Add a comment to an issue
        /// </summary>
        /// <param name="issueKey">The key of the issue (e.g., 'BWYD-123')</param>
        /// <param name="commentText">The text of the comment</param>
        /// <returns>Comment data if successful, null otherwise</returns>
        public async Task<JObject> AddCommentAsync(string issueKey, string commentText)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issue/{issueKey}/comment";
                
                // Convert comment to Jira's ADFV3 format
                var commentAdf = new JObject
                {
                    ["body"] = new JObject
                    {
                        ["type"] = "doc",
                        ["version"] = 1,
                        ["content"] = new JArray
                        {
                            new JObject
                            {
                                ["type"] = "paragraph",
                                ["content"] = new JArray
                                {
                                    new JObject
                                    {
                                        ["type"] = "text",
                                        ["text"] = commentText
                                    }
                                }
                            }
                        }
                    }
                };
                
                var content = new StringContent(commentAdf.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(responseContent);
                    Console.WriteLine($"Successfully added comment to issue: {issueKey}");
                    return data;
                }
                else
                {
                    Console.WriteLine($"Error adding comment: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when adding comment: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get available transitions for an issue
        /// </summary>
        /// <param name="issueKey">The key of the issue (e.g., 'BWYD-123')</param>
        /// <returns>List of available transitions</returns>
        public async Task<List<JObject>> GetTransitionsAsync(string issueKey)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issue/{issueKey}/transitions";
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(responseContent);
                    var transitions = data?["transitions"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                    return transitions;
                }
                else
                {
                    Console.WriteLine($"Error getting transitions: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return new List<JObject>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when getting transitions: {ex.Message}");
                return new List<JObject>();
            }
        }

        /// <summary>
        /// Transition an issue to a new status
        /// </summary>
        /// <param name="issueKey">The key of the issue (e.g., 'BWYD-123')</param>
        /// <param name="transitionId">The ID of the transition</param>
        /// <returns>True if successful, False otherwise</returns>
        public async Task<bool> TransitionIssueAsync(string issueKey, string transitionId)
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issue/{issueKey}/transitions";
                
                var payload = new JObject
                {
                    ["transition"] = new JObject
                    {
                        ["id"] = transitionId
                    }
                };
                
                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully transitioned issue: {issueKey}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error transitioning issue: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when transitioning issue: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Generate a report on project progress
        /// </summary>
        /// <returns>Dictionary with project statistics</returns>
        public async Task<JObject> GenerateProgressReportAsync()
        {
            try
            {
                // Get project issues
                var issues = await GetProjectIssuesAsync(1000);
                
                // Initialize counters
                var totalIssues = issues.Count;
                var statusCounts = new Dictionary<string, int>();
                var issueTypeCounts = new Dictionary<string, int>();
                
                // Calculate metrics
                foreach (var issue in issues)
                {
                    // Count by status
                    var status = issue["fields"]?["status"]?["name"]?.ToString();
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (!statusCounts.ContainsKey(status))
                            statusCounts[status] = 0;
                        statusCounts[status]++;
                    }
                    
                    // Count by issue type
                    var issueType = issue["fields"]?["issuetype"]?["name"]?.ToString();
                    if (!string.IsNullOrEmpty(issueType))
                    {
                        if (!issueTypeCounts.ContainsKey(issueType))
                            issueTypeCounts[issueType] = 0;
                        issueTypeCounts[issueType]++;
                    }
                }
                
                // Calculate completion percentage (if "Done" status exists)
                var completionPercentage = 0.0;
                if (statusCounts.ContainsKey("Done") && totalIssues > 0)
                {
                    completionPercentage = (double)statusCounts["Done"] / totalIssues * 100;
                }
                
                // Create report
                var report = new JObject
                {
                    ["timestamp"] = DateTime.UtcNow.ToString("o"),
                    ["project_key"] = _projectKey,
                    ["total_issues"] = totalIssues,
                    ["status_breakdown"] = JObject.FromObject(statusCounts),
                    ["issue_type_breakdown"] = JObject.FromObject(issueTypeCounts),
                    ["completion_percentage"] = completionPercentage
                };
                
                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when generating report: {ex.Message}");
                return new JObject { ["error"] = ex.Message };
            }
        }

        /// <summary>
        /// Get all available issue types in the Jira instance
        /// </summary>
        /// <returns>List of available issue types</returns>
        public async Task<List<JObject>> GetIssueTypesAsync()
        {
            try
            {
                var url = $"{_jiraUrl}/rest/api/3/issuetype";
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<JObject>>(responseContent);
                    Console.WriteLine($"Found {data?.Count ?? 0} issue types");
                    return data ?? new List<JObject>();
                }
                else
                {
                    Console.WriteLine($"Error fetching issue types: {(int)response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return new List<JObject>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when fetching issue types: {ex.Message}");
                return new List<JObject>();
            }
        }

        /// <summary>
        /// Dispose the HttpClient when done
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}