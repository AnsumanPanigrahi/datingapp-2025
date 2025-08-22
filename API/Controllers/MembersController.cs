using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // https://localhost/api/members
    public class MembersController(AppDbContext context) : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return members;
        }

        [Authorize]
        [HttpGet("{id}")] // https://localhost/api/members/bob-id
        public async Task<ActionResult<AppUser?>> GetMember(string id)
        {
            var member = await context.Users.FirstOrDefaultAsync(member => member.Id == id);
            if (member == null)
                return NotFound();

            return member;
        }
    }
}
