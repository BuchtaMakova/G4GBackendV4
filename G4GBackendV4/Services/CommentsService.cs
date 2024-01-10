using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using Microsoft.EntityFrameworkCore;

namespace G4GBackendV4.Services
{
    public class CommentsService
    {
        private readonly G4GDbContext _context;

        public CommentsService(G4GDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommentDto>> GetCommentsFromDatabase(int contentIdContent)
        {
            var comments = _context.Comments;

            if (contentIdContent == 0)
            {
                var allComments = await comments.ToListAsync();
                return allComments.Select(comment => GetCommentDto(comment, _context)).ToList();
            }

            var filteredComments = await comments.Where(comment => comment.ContentId == contentIdContent).ToListAsync();
            return filteredComments.Select(comment => GetCommentDto(comment, _context)).ToList();
        }

        public static CommentDto GetCommentDto(Comment comment, G4GDbContext context)
        {
            var user = context.Users.First(user => user.Id == comment.UserId);
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

        public static AccountDto GetAccountDto(User user, G4GDbContext context)
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
    }
}