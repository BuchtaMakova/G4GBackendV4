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
    public class UsersController : ControllerBase
    {
        private readonly G4GDbContext _context;
        private readonly SecurityService _securityService;
        private readonly UserService _userService;

        public UsersController([FromServices] SecurityService securityService, [FromServices] UserService userService,
            G4GDbContext context)
        {
            _securityService = securityService;
            _userService = userService;
            _context = context;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto login)
        {
            User user;
            try
            {
                user = await _userService.GetUserByCredentials(login.Username, login.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_securityService.BuildJwtToken(user));
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserLoginDto user)
        {
            return Ok(_securityService.BuildJwtToken(await _userService.Create(user.Username!, user.Password!)));
        }

        [HttpGet("ByUsername")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<ActionResult> GetAccount(string name)
        {
            if (_context.Users != null)
            {
                User ac = await _userService.GetByUsername(name);

                var comments = ac.Comments
                    .Where(cm => cm.User.Username == ac.Username)
                    .Select(cn => new CommentDto
                    {
                        AccountIdAccount = cn.UserId,
                        AccountUsername = cn.User!.Username,
                        ContentIdContent = cn.ContentId,
                        IdComment = cn.Id,
                        Posted = cn.Posted,
                        Text = cn.Text
                    })
                    .ToList();

                var contents = ac.Contents
                    .Where(ct => ct.User.Username == ac.Username)
                    .Select(cn => new ContentDto
                    {
                        Account = new AccountDto
                        {
                            IdAccount = cn.UserId,
                            Username = cn.User.Username,
                            CommentsPosted = cn.User.Comments.Count(q => q.UserId == cn.UserId),
                            ContentsPosted = cn.User.Contents.Count(q => q.UserId == cn.UserId),
                        },
                        Headline = cn.Headline,
                        IdContent = cn.Id,
                        Posted = cn.Posted,
                        SubcategoryIdSubcategory = cn.SubcategoryId,
                        Text = cn.Text,
                        Views = cn.Views
                    })
                    .ToList();

                return Ok(new AccountDto
                {
                    IdAccount = ac.Id,
                    Username = ac.Username,
                    CommentsPosted = comments.Count,
                    Comments = comments,
                    ContentsPosted = contents.Count,
                    Contents = contents
                });
            }

            return BadRequest();
        }
    }
}
