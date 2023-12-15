using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4GBackendV4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContentsController : ControllerBase
    {
        private readonly G4GDbContext _context;

        public ContentsController(G4GDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetContents")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContents(int subcategoryIdSubcategory)
        {
            if (subcategoryIdSubcategory == 0)
                return Ok(await _context.Contents.Include(cm => cm.Comments).Select(cn => new ContentDto
                {
                    Account = _context.Users.Select(ac => new AccountDto
                    {
                        IdAccount = ac.Id,
                        Username = ac.Username,
                        CommentsPosted = ac.Comments.Select(cn => new CommentDto
                        {
                            AccountIdAccount = cn.UserId,
                            AccountUsername = cn.User.Username,
                            ContentIdContent = cn.ContentId,
                            IdComment = cn.Id,
                            Posted = cn.Posted,
                            Text = cn.Text
                        }).Count(cm => cm.AccountUsername == ac.Username),
                        ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id)
                    }).First(ac => ac.Username == cn.User.Username),
                    Headline = cn.Headline,
                    IdContent = cn.Id,
                    Posted = cn.Posted,
                    SubcategoryIdSubcategory = cn.SubcategoryId,
                    Text = cn.Text,
                    Views = cn.Views,
                    Comment = (ICollection<CommentDto>)cn.Comments.Select(cn => new CommentDto
                    {
                        Account = _context.Users.Select(ac => new AccountDto
                        {
                            IdAccount = ac.Id,
                            Username = ac.Username,
                            CommentsPosted = ac.Comments.Select(cn => new CommentDto
                            {
                                AccountIdAccount = cn.UserId,
                                AccountUsername = cn.User.Username,
                                ContentIdContent = cn.ContentId,
                                IdComment = cn.Id,
                                Posted = cn.Posted,
                                Text = cn.Text
                            }).Count(cm => cm.AccountUsername == ac.Username),
                            ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id)
                        }).First(ac => ac.Username == cn.User.Username),
                        AccountIdAccount = cn.UserId,
                        AccountUsername = cn.User.Username,
                        ContentIdContent = cn.ContentId,
                        IdComment = cn.Id,
                        Posted = cn.Posted,
                        Text = cn.Text
                    }),
                    CommentsCount = cn.Comments.Select(cn => new CommentDto
                    {
                        AccountIdAccount = cn.UserId,
                        AccountUsername = cn.User.Username,
                        ContentIdContent = cn.ContentId,
                        IdComment = cn.Id,
                        Posted = cn.Posted,
                        Text = cn.Text
                    }).Count()
                }).OrderByDescending(ct => ct.IdContent).ToListAsync());
            return Ok(await _context.Contents.Include(cm => cm.Comments).Select(cn => new ContentDto
            {
                Account = _context.Users.Select(ac => new AccountDto
                {
                    IdAccount = ac.Id,
                    Username = ac.Username,
                    CommentsPosted = ac.Comments.Count(q => q.UserId == ac.Id),
                    ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id),
                }).First(ac => ac.Username == cn.User.Username),
                Headline = cn.Headline,
                IdContent = cn.Id,
                Posted = cn.Posted,
                SubcategoryIdSubcategory = cn.SubcategoryId,
                Text = cn.Text,
                Views = cn.Views,
                Comment = (ICollection<CommentDto>)cn.Comments.Select(cn => new CommentDto
                {
                    Account = _context.Users.Select(ac => new AccountDto
                    {
                        IdAccount = ac.Id,
                        Username = ac.Username,
                        CommentsPosted = ac.Comments.Count(q => q.UserId == ac.Id),
                        ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id)
                    }).First(ac => ac.Username == cn.User.Username),
                    AccountIdAccount = cn.UserId,
                    AccountUsername = cn.User.Username,
                    ContentIdContent = cn.ContentId,
                    IdComment = cn.Id,
                    Posted = cn.Posted,
                    Text = cn.Text
                }),
                CommentsCount = cn.Comments.Select(cn => new CommentDto
                {
                    AccountIdAccount = cn.UserId,
                    AccountUsername = cn.User.Username,
                    ContentIdContent = cn.ContentId,
                    IdComment = cn.Id,
                    Posted = cn.Posted,
                    Text = cn.Text
                }).Count()
            }).Where(cn => cn.SubcategoryIdSubcategory == subcategoryIdSubcategory)
                .OrderByDescending(ct => ct.IdContent)
                .ToListAsync());
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _context.Contents.Include(cm => cm.Comments).Select(cn => new ContentDto
            {
                Account = _context.Users.Select(ac => new AccountDto
                {
                    IdAccount = ac.Id,
                    Username = ac.Username,
                    CommentsPosted = ac.Comments.Select(cn => new CommentDto
                    {
                        AccountIdAccount = cn.UserId,
                        AccountUsername = cn.User.Username,
                        ContentIdContent = cn.ContentId,
                        IdComment = cn.Id,
                        Posted = cn.Posted,
                        Text = cn.Text
                    }).Count(cm => cm.AccountUsername == ac.Username),
                    ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id)
                }).First(ac => ac.Username == cn.User.Username),
                Headline = cn.Headline,
                IdContent = cn.Id,
                Posted = cn.Posted,
                SubcategoryIdSubcategory = cn.SubcategoryId,
                Text = cn.Text,
                Views = cn.Views,
                Comment = (ICollection<CommentDto>)cn.Comments.Select(cn => new CommentDto
                {
                    Account = _context.Users.Select(ac => new AccountDto
                    {
                        IdAccount = ac.Id,
                        Username = ac.Username,
                        CommentsPosted = ac.Comments.Select(cn => new CommentDto
                        {
                            AccountIdAccount = cn.UserId,
                            AccountUsername = cn.User.Username,
                            ContentIdContent = cn.ContentId,
                            IdComment = cn.Id,
                            Posted = cn.Posted,
                            Text = cn.Text
                        }).Count(cm => cm.AccountUsername == ac.Username),
                        ContentsPosted = ac.Contents.Count(q => q.UserId == ac.Id)
                    }).First(ac => ac.Username == cn.User.Username),
                    AccountIdAccount = cn.UserId,
                    AccountUsername = cn.User.Username,
                    ContentIdContent = cn.ContentId,
                    IdComment = cn.Id,
                    Posted = cn.Posted,
                    Text = cn.Text
                }),
                CommentsCount = cn.Comments.Select(cn => new CommentDto
                {
                    AccountIdAccount = cn.UserId,
                    AccountUsername = cn.User.Username,
                    ContentIdContent = cn.ContentId,
                    IdComment = cn.Id,
                    Posted = cn.Posted,
                    Text = cn.Text
                }).Count()
            }).FirstAsync(q => q.IdContent == id));
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
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

            _context.Contents?.Add(cont);
            await _context.SaveChangesAsync();

            return Ok(cont);
        }

        [HttpPut("Update")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Update(UpdateContentDto content)
        {
            var cont = await _context.Contents!.FindAsync(content.Id);
            if (cont == null) return NotFound();
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
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Delete(long id)
        {
            var content = await _context.Contents!.FindAsync(id);
            if (content == null) return NotFound();

            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
