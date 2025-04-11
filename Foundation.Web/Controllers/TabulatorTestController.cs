using System.Linq;
using System.Linq.Dynamic.Core;
using Foundation.Components.Models;
using Foundation.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TabulatorTestController : ControllerBase
    {
        private static readonly List<TestUser> AllUsers = Enumerable.Range(1, 100).Select(i => new TestUser
        {
            Id = i,
            Name = $"User {i}",
            Email = $"user{i}@example.com"
        }).ToList();


        [HttpPost]
        public IActionResult GetData(TabulatorRequest request)
        {
            IQueryable<TestUser> query = AllUsers.AsQueryable();

            foreach (var filter in request.Filter)
            {
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    query = query.Where($"{filter.Field}.Contains(@0)", filter.Value);
                }
            }

            foreach (var sorter in request.Sort)
            {
                string order = sorter.Dir.ToLower() == "desc" ? "descending" : "ascending";
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
