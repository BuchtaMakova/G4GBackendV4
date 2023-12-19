using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G4GBackendV4.Services
{
    public class ContentService
    {
        private readonly G4GDbContext _context;

        public ContentService(G4GDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ContentDto> GetContents(int subcategoryIdSubcategory)
        {
            var query = _context.Contents
                .Include(cm => cm.Comments)
                .Include(q=>q.User)
                .Where(cn => cn.SubcategoryId == subcategoryIdSubcategory || subcategoryIdSubcategory == 0)
                .OrderByDescending(ct => ct.Id)
                .Select(MapContentToDto);

            return query.ToList(); 
        }

        public async Task<ContentDto> GetById(int id)
        {
            var content = await _context.Contents
                .Include(cm => cm.Comments)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (content == null)
            {
                return null; 
            }

            return MapContentToDto(content);
        }

        public async Task<IActionResult> Create(PostContentDto content)
        {
            var cont = new Content
            {
                Headline = content.Headline,
                Text = content.Text,
                Posted = content.Posted,
                UserId = content.AccountIdAccount,
                SubcategoryId = content.SubcategoryIdSubcategory
            };

            _context.Contents.Add(cont);
            await _context.SaveChangesAsync();

            var createdContentDto = MapContentToDto(cont);
            return new ObjectResult(createdContentDto) { StatusCode = 201 };
        }

        public async Task<IActionResult> Update(UpdateContentDto content)
        {
            var cont = await _context.Contents.FindAsync(content.Id);
            if (cont == null)
            {
                return new NotFoundResult();
            }

            cont.Headline = content.Headline;
            cont.Text = content.Text;
            cont.UserId = content.AccountIdAccount;
            cont.SubcategoryId = content.SubcategoryIdSubcategory;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                 return new NotFoundResult();
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> Delete(long id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return new NotFoundResult();
            }

            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private static ContentDto MapContentToDto(Content content)
        {
            return new ContentDto
            {
                IdContent = content.Id,
                Headline = content.Headline,
                Text = content.Text,
                Posted = content.Posted,
                SubcategoryIdSubcategory = content.SubcategoryId,
                Views = content.Views,
                // Map other properties as needed

                Account = MapAccountToDto(content.User),
                Comment = MapCommentsToDto(content.Comments),

                CommentsCount = content.Comments.Count
            };
        }

        private static AccountDto MapAccountToDto(User user)
        {
            if (user == null)
            {
               
                return new AccountDto(); 
            }

            return new AccountDto
            {
                IdAccount = user.Id,
                Username = user.Username,
                CommentsPosted = user.Comments?.Count(q => q.UserId == user.Id) ?? 0,
                ContentsPosted = user.Contents?.Count(q => q.UserId == user.Id) ?? 0
                
            };
        }

        private static ICollection<CommentDto> MapCommentsToDto(ICollection<Comment> comments)
        {
            return comments.Select(cn => new CommentDto
            {
                Account = MapAccountToDto(cn.User),
                AccountIdAccount = cn.UserId,
                AccountUsername = cn.User?.Username,
                ContentIdContent = cn.ContentId,
                IdComment = cn.Id,
                Posted = cn.Posted,
                Text = cn.Text
               
            }).ToList();
        }
    }
}