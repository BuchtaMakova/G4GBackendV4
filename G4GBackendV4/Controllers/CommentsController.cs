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
        private readonly ContextService _context;

        public CommentsController(ContextService context)
        {
            _context = context;
        }

        [HttpGet("GetComments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int contentIdContent)
        {
            var comments = await GetCommentsFromDatabase(contentIdContent);
            return Ok(comments);
        }

        private async Task<List<CommentDto>> GetCommentsFromDatabase(int contentIdContent)
        {
            var comments = _context.GetContext().Comments;
            if (contentIdContent == 0)
            {
                return await comments.Select(comment => GetCommentDto(comment, _context)).ToListAsync();
            }
            return await comments.Where(comment => comment.ContentId == contentIdContent)
                              .Select(comment => GetCommentDto(comment, _context))
                              .ToListAsync();
        }

        private static CommentDto GetCommentDto(Comment comment, ContextService context)
        {
            var user = context.GetContext().Users.First(user => user.Id == comment.UserId);
            var accountDto = GetAccountDto(user, context);

            return new CommentDto
            {
                IdComment = comment.Id,
                ContentIdContent = comment.ContentId,
                Text = comment.Text,
                Posted = comment.Posted,
                Account = accountDto
            };
        }

        private static AccountDto GetAccountDto(User user, ContextService context)
        {
            var commentsPosted = user.Comments.Count(comment => comment.UserId == user.Id);
            var contentsPosted = user.Contents.Count(content => content.UserId == user.Id);

            return new AccountDto
            {
                IdAccount = user.Id,
                Username = user.Username,
                CommentsPosted = commentsPosted,
                ContentsPosted = contentsPosted
            };
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

            _context.GetContext().Comments?.Add(comm);
            await _context.GetContext().SaveChangesAsync();

            return Ok(comm);
        }

        [HttpPut("Update")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Update(UpdateCommentDto comment)
        {
            var comm = await _context.GetContext().Comments!.FindAsync(comment.Id);
            if (comm == null) return NotFound();
            comm.Text = comment.Text;
            comm.UserId = comment.AccountIdAccount;
            comm.ContentId = comment.ContentIdContent;
            try
            {
                await _context.GetContext().SaveChangesAsync();
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
            var comment = await _context.GetContext().Comments!.FindAsync(id);
            if (comment == null) return NotFound();

            _context.GetContext().Comments?.Remove(comment);
            await _context.GetContext().SaveChangesAsync();

            return NoContent();
        }
    }
}
