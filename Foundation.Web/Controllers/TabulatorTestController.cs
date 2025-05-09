using System.Linq.Dynamic.Core;
using Foundation.Components.Controllers;
using Foundation.Components.Models;
using Foundation.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// API controller for testing Tabulator.js integration with server-side filtering, sorting, and pagination.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TabulatorTestController : FoundationBaseController
    {
        private static readonly List<TestUser> AllUsers = [.. Enumerable.Range(1, 100).Select(i => new TestUser
        {
            Id = i,
            Name = $"User {i}",
            Email = $"user{i}@example.com"
        })];

        /// <summary>
        /// Initializes a new instance of the <see cref="TabulatorTestController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for the controller.</param>
        public TabulatorTestController(ILogger<TabulatorTestController> logger) : base(logger)
        {
        }

        /// <summary>
        /// Handles Tabulator AJAX requests, applying filtering, sorting, and pagination to the sample <see cref="TestUser"/> list.
        /// </summary>
        /// <param name="request">The Tabulator request object containing pagination, filter, and sort parameters.</param>
        /// <returns>
        /// A JSON result containing a page of filtered and sorted <see cref="TestUser"/> data and the total number of pages.
        /// </returns>
        /// <response code="200">Returns the filtered and paginated user data.</response>
        /// <response code="400">If the request is null or invalid.</response>
        [HttpPost]
        public IActionResult GetData(TabulatorRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            IQueryable<TestUser> query = AllUsers.AsQueryable();

            var properties = typeof(TestUser)
            .GetProperties()
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            string combinedFilter = string.Empty;

            foreach (var filter in request.Filter)
            {
                if (!string.IsNullOrEmpty(filter.Value) && properties.TryGetValue(filter.Field, out var property))
                {
                    string filterExpression = string.Empty;

                    if (property.PropertyType == typeof(string))
                    {
                        filterExpression = $"{filter.Field}.Contains(@0)";
                    }
                    else if (property.PropertyType == typeof(int) ||
                             property.PropertyType == typeof(long) ||
                             property.PropertyType == typeof(decimal) ||
                             property.PropertyType == typeof(double) ||
                             property.PropertyType == typeof(float))
                    {
                        if (decimal.TryParse(filter.Value, out var numberValue))
                        {
                            filterExpression = $"{filter.Field} == @0";
                        }
                    }
                    else if (property.PropertyType == typeof(Guid))
                    {
                        if (Guid.TryParse(filter.Value, out var guidValue))
                        {
                            filterExpression = $"{filter.Field} == @0";
                        }
                    }
                    else
                    {
                        // fallback: treat like string
                        filterExpression = $"{filter.Field}.ToString().Contains(@0)";
                    }

                    if (!string.IsNullOrEmpty(filterExpression))
                    {
                        if (!string.IsNullOrEmpty(combinedFilter))
                        {
                            combinedFilter = $"({combinedFilter}) Or ({filterExpression})";
                        }
                        else
                        {
                            combinedFilter = filterExpression;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(combinedFilter))
            {
                query = query.Where(combinedFilter, [.. request.Filter.Select(f => f.Value)]);
            }

            foreach (var sorter in request.Sort)
            {
                string order = string.Equals(sorter.Dir, "desc", StringComparison.OrdinalIgnoreCase)
                    ? "descending"
                    : "ascending";

                query = query.OrderBy($"{sorter.Field} {order}");
            }

            int total = query.Count();
            var data = query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToList();

            return Ok(new TabulatorResponse<TestUser>
            {
                LastPage = (int)Math.Ceiling((double)total / request.Size),
                Data = data
            });
        }

    }
}
