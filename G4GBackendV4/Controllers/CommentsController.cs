using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using G4GBackendV4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4GBackendV4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly G4GDbContext _context;
        private readonly CommentsService _commentsService; 

        public CommentsController(G4GDbContext context, CommentsService commentsService)
        {
            _context = context;
            _commentsService = commentsService;
        }

        [HttpGet("GetComments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int contentIdContent)
        {
            var comments = await _commentsService.GetCommentsFromDatabase(contentIdContent);
            return Ok(comments);
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Create(PostCommentDto comment)
        {
            var comm = new Comment
            {
                Text = comment.Text,
                Posted = comment.Posted,
                UserId = comment.AccountIdAccount,
                ContentId = comment.ContentIdContent
            };

            _context.Comments?.Add(comm);
            await _context.SaveChangesAsync();

            return Ok(comm);
        }

        [HttpPut("Update")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Update(UpdateCommentDto comment)
        {
            var comm = await _context.Comments!.FindAsync(comment.Id);
            if (comm == null) return NotFound();
            comm.Text = comment.Text;
            comm.UserId = comment.AccountIdAccount;
            comm.ContentId = comment.ContentIdContent;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Delete(long id)
        {
            var comment = await _context.Comments!.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments?.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
