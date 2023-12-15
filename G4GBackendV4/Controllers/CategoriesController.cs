using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using G4GBackendV4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G4GBackendV4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ContextService _context;

        public CategoriesController(ContextService context)
        {
            _context = context;
        }

        public static int CalculateTotalComments(List<ContentDto> contents)
        {
            return contents.Sum(content => content.Comment.Count());
        }

        [HttpGet("GetCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.GetContext().Categories.Include(x => x.SubCategories).ToListAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                IdCategory = c.Id,
                Name = c.Name,
                SubCategory = c.SubCategories.Select(sc => CreateSubCategoryDto(sc)).ToList()
            }).ToList();
            return Ok(categoriesDto);
        }

        private SubCategoryDto CreateSubCategoryDto(SubCategory sc)
        {
            var lastContent = GetLastContentInSubCategory((int)sc.Id);
            var totalComments = _context.GetContext().Contents.Count(cn => cn.SubcategoryId == sc.Id);
            var totalContents = _context.GetContext().Contents.Count(cm => cm.SubcategoryId == sc.Id);

            return new SubCategoryDto
            {
                Icon = sc.Icon,
                IdSubcategory = sc.Id,
                Name = sc.Name,
                lastContentInSubCategory = lastContent,
                totalCommentInInSubCategory = totalComments,
                totalContentsInSubCategory = totalContents
            };
        }

        private ContentDto GetLastContentInSubCategory(int subcategoryId)
        {
            return _context.GetContext().Contents
                .Include(cm => cm.Comments)
                .Where(cn => cn.SubcategoryId == subcategoryId)
                .OrderByDescending(cn => cn.Id)
                .Select(cn => new ContentDto
                {
                    IdContent = cn.Id,
                    Headline = cn.Headline,
                    Text = cn.Text,
                    Views = cn.Views,
                    Posted = cn.Posted,
                    Comment = cn.Comments.Select(cm => new CommentDto
                    {
                        IdComment = cm.Id,
                        Text = cm.Text,
                        Posted = cm.Posted,
                        AccountIdAccount = cm.UserId
                    }).ToList(),
                    Account = new AccountDto
                    {
                        Username = cn.User.Username,
                        IdAccount = cn.UserId,
                    }
                }).FirstOrDefault();
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> Create(string name)
        {
            var category = new Category { Name = name, };

            await _context.GetContext().Categories.AddAsync(category);
            await _context.GetContext().SaveChangesAsync();

            return Ok(category);
        }
    }
}