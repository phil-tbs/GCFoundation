using System.Diagnostics;
using System.Text.Json;

namespace GCFoundation.Tests.McpServer.Helpers
{
    public static class McpTestHelper
    {
        /// <summary>
        /// Creates a standardized test request for MCP tool calls
        /// </summary>
        public static string CreateToolCallRequest(string toolName, object arguments, int id = 1)
        {
            var request = new
            {
                jsonrpc = "2.0",
                id = id,
                method = "tools/call",
                @params = new
                {
                    name = toolName,
                    arguments = arguments
                }
            };

            return JsonSerializer.Serialize(request, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
        }

        /// <summary>
        /// Creates a tools/list request
        /// </summary>
        public static string CreateToolsListRequest(int id = 1)
        {
            var request = new
            {
                jsonrpc = "2.0",
                id = id,
                method = "tools/list",
                @params = new { }
            };

            return JsonSerializer.Serialize(request);
        }

        /// <summary>
        /// Validates that a response is valid JSON-RPC format
        /// </summary>
        public static bool IsValidJsonRpcResponse(string response)
        {
            try
            {
                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                return root.TryGetProperty("jsonrpc", out var jsonrpc) &&
                       jsonrpc.GetString() == "2.0" &&
                       (root.TryGetProperty("result", out _) || root.TryGetProperty("error", out _)) &&
                       root.TryGetProperty("id", out _);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts the result from a JSON-RPC response
        /// </summary>
        public static string? ExtractResult(string response)
        {
            try
            {
                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                if (root.TryGetProperty("result", out var result))
                {
                    return result.TryGetProperty("content", out var content) 
                        ? content.GetString() 
                        : result.GetRawText();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Validates HTML/XML markup structure
        /// </summary>
        public static bool IsValidMarkup(string markup)
        {
            try
            {
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml($"<root>{markup}</root>");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Common test arguments for different tool types
        /// </summary>
        public static class TestArguments
        {
            public static readonly object GcdsButton = new { text = "Test Button", variant = "primary" };
            public static readonly object GcdsInput = new { name = "test", label = "Test Field", type = "text", required = true };
            public static readonly object GcdsSelect = new { name = "test", label = "Test Select", options = "1:Option 1,2:Option 2" };
            public static readonly object FdcpCard = new { content = "Test Content", header = "Test Header" };
            public static readonly object FdcpModal = new { modalId = "test", title = "Test Modal", content = "Test Content" };
            public static readonly object FdcpTable = new { tableId = "test", columnsJson = "[{\"title\":\"Name\",\"field\":\"name\"}]" };
            public static readonly object ProjectConfig = new { includeAuthentication = true, includeLocalization = true };
        }

        /// <summary>
        /// Gets the full tool name with MCP prefix
        /// </summary>
        public static string GetFullToolName(string toolName)
        {
            return toolName.StartsWith("mcp_") ? toolName : $"mcp_GCFoundation_McpServer_{toolName}";
        }

        /// <summary>
        /// Performance test helper - measures tool execution time
        /// </summary>
        public static async Task<TimeSpan> MeasureToolPerformance(Func<Task> toolExecution, int iterations = 10)
        {
            var sw = Stopwatch.StartNew();
            
            for (int i = 0; i < iterations; i++)
            {
                await toolExecution();
            }
            
            sw.Stop();
            return TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds / (double)iterations);
        }

        /// <summary>
        /// Validates accessibility attributes in generated markup
        /// </summary>
        public static List<string> ValidateAccessibility(string markup)
        {
            var issues = new List<string>();

            // Check for required form attributes
            if (markup.Contains("<gcds-input") || markup.Contains("<fdcp-input"))
            {
                if (!markup.Contains("label="))
                    issues.Add("Input missing label attribute");
                
                if (!markup.Contains("name="))
                    issues.Add("Input missing name attribute");
            }

            // Check for button accessibility
            if (markup.Contains("<gcds-button") || markup.Contains("<fdcp-button"))
            {
                if (markup.Contains("onclick="))
                    issues.Add("Button uses inline onclick (prefer event listeners)");
            }

            // Check for proper heading structure
            if (markup.Contains("<gcds-heading"))
            {
                if (!markup.Contains("tag="))
                    issues.Add("Heading missing semantic tag attribute");
            }

            return issues;
        }

        /// <summary>
        /// Validates Government of Canada design system compliance
        /// </summary>
        public static List<string> ValidateGCCompliance(string markup)
        {
            var issues = new List<string>();

            // Check for proper GCDS component usage
            if (markup.Contains("class=\"btn"))
                issues.Add("Using bootstrap classes instead of GCDS components");

            if (markup.Contains("class=\"form-control"))
                issues.Add("Using bootstrap form classes instead of GCDS components");

            // Check for bilingual support
            if (markup.Contains("label=") && !markup.Contains("lang="))
                issues.Add("Consider adding language attributes for bilingual support");

            // Check for proper error handling
            if (markup.Contains("required=\"true\"") && !markup.Contains("error-message"))
                issues.Add("Required field should have error message capability");

            return issues;
        }
    }
}
