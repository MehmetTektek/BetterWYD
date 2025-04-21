using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BetterWYD.DevTools.JiraManager
{
    /// <summary>
    /// JiraManagerUnity - Unity integration component for the JiraManager API
    /// 
    /// This MonoBehaviour provides a convenient way to interact with Jira directly 
    /// from Unity. It wraps the JiraManagerAPI and provides both inspector-configurable
    /// settings and runtime methods for managing Jira tasks within the game development process.
    /// 
    /// Features:
    /// - Configure Jira connection details in the Unity Inspector
    /// - Create, update, and comment on Jira issues from Unity editor or runtime
    /// - Transition issues between statuses
    /// - Generate progress reports on Jira projects
    /// 
    /// Created as part of the BetterWYD project infrastructure on April 21, 2025
    /// </summary>
    public class JiraManagerUnity : MonoBehaviour
    {
        [Header("Jira Connection Settings")]
        [Tooltip("Email address associated with your Jira account")]
        [SerializeField] private string jiraEmail = "";
        
        [Tooltip("API token generated from your Atlassian account")]
        [SerializeField] private string apiToken = "";
        
        [Tooltip("URL of your Jira instance (e.g., https://your-domain.atlassian.net)")]
        [SerializeField] private string jiraUrl = "";
        
        [Tooltip("Key of the Jira project (e.g., BWYD)")]
        [SerializeField] private string projectKey = "BWYD";

        [Header("Debug Options")]
        [Tooltip("Whether to test connection on startup")]
        [SerializeField] private bool testConnectionOnStart = false;

        [Tooltip("Log API operations to console")]
        [SerializeField] private bool logOperations = true;

        // Reference to the API instance
        private JiraManagerAPI _api;
        private bool _isConnected = false;

        // Singleton instance for easy access
        private static JiraManagerUnity _instance;
        public static JiraManagerUnity Instance => _instance;

        private void Awake()
        {
            // Simple singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize the API
            InitializeAPI();
        }

        private void Start()
        {
            if (testConnectionOnStart)
            {
                TestConnection();
            }
        }

        private void OnDestroy()
        {
            // Clean up resources when destroyed
            _api?.Dispose();
        }

        /// <summary>
        /// Initialize the JiraManagerAPI with credentials
        /// </summary>
        private void InitializeAPI()
        {
            try
            {
                _api = new JiraManagerAPI(jiraEmail, apiToken, jiraUrl, projectKey);
                
                if (logOperations)
                    Debug.Log("[JiraManager] API initialized");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Failed to initialize API: {ex.Message}");
            }
        }

        /// <summary>
        /// Test the connection to Jira
        /// </summary>
        public async void TestConnection()
        {
            if (_api == null)
            {
                Debug.LogError("[JiraManager] API not initialized");
                return;
            }

            try
            {
                var result = await _api.TestConnectionAsync();
                _isConnected = result;
                
                if (result)
                {
                    Debug.Log("[JiraManager] Successfully connected to Jira");
                }
                else
                {
                    Debug.LogError("[JiraManager] Failed to connect to Jira");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error testing connection: {ex.Message}");
            }
        }

        /// <summary>
        /// Get issues from the Jira project
        /// </summary>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <param name="callback">Callback function that will be called with the results</param>
        public async void GetProjectIssues(int maxResults = 50, Action<List<JObject>> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var issues = await _api.GetProjectIssuesAsync(maxResults);
                
                if (logOperations)
                    Debug.Log($"[JiraManager] Retrieved {issues.Count} issues");
                
                callback?.Invoke(issues);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error getting issues: {ex.Message}");
                callback?.Invoke(new List<JObject>());
            }
        }

        /// <summary>
        /// Create a new issue in the Jira project
        /// </summary>
        /// <param name="summary">Issue summary/title</param>
        /// <param name="description">Detailed description</param>
        /// <param name="issueType">Type of issue (e.g., "Task", "Bug")</param>
        /// <param name="callback">Callback function that will be called with the created issue data</param>
        public async void CreateIssue(string summary, string description, string issueType = "Task", Action<JObject> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var issue = await _api.CreateIssueAsync(summary, description, issueType);
                
                if (logOperations)
                {
                    if (issue != null)
                        Debug.Log($"[JiraManager] Created issue {issue["key"]}");
                    else
                        Debug.LogWarning("[JiraManager] Failed to create issue");
                }
                
                callback?.Invoke(issue);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error creating issue: {ex.Message}");
                callback?.Invoke(null);
            }
        }

        /// <summary>
        /// Add a comment to an issue
        /// </summary>
        /// <param name="issueKey">The key of the issue (e.g., 'BWYD-123')</param>
        /// <param name="commentText">The text of the comment</param>
        /// <param name="callback">Callback function that will be called with success status</param>
        public async void AddComment(string issueKey, string commentText, Action<bool> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var result = await _api.AddCommentAsync(issueKey, commentText);
                bool success = result != null;
                
                if (logOperations)
                {
                    if (success)
                        Debug.Log($"[JiraManager] Added comment to issue {issueKey}");
                    else
                        Debug.LogWarning($"[JiraManager] Failed to add comment to issue {issueKey}");
                }
                
                callback?.Invoke(success);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error adding comment: {ex.Message}");
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// Update an existing issue
        /// </summary>
        /// <param name="issueKey">The key of the issue to update (e.g., 'BWYD-123')</param>
        /// <param name="fieldsToUpdate">Dictionary of field keys and values to update</param>
        /// <param name="callback">Callback function that will be called with success status</param>
        public async void UpdateIssue(string issueKey, Dictionary<string, object> fieldsToUpdate, Action<bool> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var success = await _api.UpdateIssueAsync(issueKey, fieldsToUpdate);
                
                if (logOperations)
                {
                    if (success)
                        Debug.Log($"[JiraManager] Updated issue {issueKey}");
                    else
                        Debug.LogWarning($"[JiraManager] Failed to update issue {issueKey}");
                }
                
                callback?.Invoke(success);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error updating issue: {ex.Message}");
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// Transition an issue to a new status
        /// </summary>
        /// <param name="issueKey">The key of the issue (e.g., 'BWYD-123')</param>
        /// <param name="transitionId">The ID of the transition</param>
        /// <param name="callback">Callback function that will be called with success status</param>
        public async void TransitionIssue(string issueKey, string transitionId, Action<bool> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var success = await _api.TransitionIssueAsync(issueKey, transitionId);
                
                if (logOperations)
                {
                    if (success)
                        Debug.Log($"[JiraManager] Transitioned issue {issueKey}");
                    else
                        Debug.LogWarning($"[JiraManager] Failed to transition issue {issueKey}");
                }
                
                callback?.Invoke(success);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error transitioning issue: {ex.Message}");
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// Generate a report on project progress
        /// </summary>
        /// <param name="callback">Callback function that will be called with the report</param>
        public async void GenerateProgressReport(Action<JObject> callback = null)
        {
            if (_api == null || !EnsureConnected())
                return;

            try
            {
                var report = await _api.GenerateProgressReportAsync();
                
                if (logOperations)
                    Debug.Log($"[JiraManager] Generated progress report");
                
                callback?.Invoke(report);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error generating report: {ex.Message}");
                callback?.Invoke(new JObject { ["error"] = ex.Message });
            }
        }

        /// <summary>
        /// Ensure the API is connected to Jira
        /// </summary>
        /// <returns>True if connected or connection successful, false otherwise</returns>
        private async Task<bool> EnsureConnectedAsync()
        {
            if (_isConnected)
                return true;
            
            try
            {
                _isConnected = await _api.TestConnectionAsync();
                return _isConnected;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JiraManager] Error connecting to Jira: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Ensure the API is connected to Jira (non-async version)
        /// </summary>
        /// <returns>True if already connected, false otherwise (will attempt to connect in background)</returns>
        private bool EnsureConnected()
        {
            if (_isConnected)
                return true;
            
            // Start connection attempt in background
            Task.Run(async () => await EnsureConnectedAsync());
            
            // Return false since we're not connected yet (the background task will update _isConnected)
            return false;
        }
    }
}