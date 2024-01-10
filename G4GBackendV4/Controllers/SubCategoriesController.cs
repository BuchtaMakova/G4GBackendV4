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
    public class SubCategoriesController : ControllerBase
    {
        private readonly G4GDbContext _context;

        public SubCategoriesController(G4GDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> Create(PostSubCategoryDto subCat)
        {
            var sub = new SubCategory { Name = subCat.Name!, Icon = subCat.Icon!, CategoryId = subCat.CategoryId };
            _context.Add(sub);
            await _context.SaveChangesAsync();
            var cont = new Content
            {
                Headline = "New subcategory",
                Text = "Just launched new subcategory",
                Posted = DateTime.Now,
                UserId = 1,
                SubcategoryId = sub.Id,
                Views = 0
            };
            _context.Add(cont);
            await _context.SaveChangesAsync();


            return Ok(sub);
        }

        [HttpGet("GetSubcategories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategory(int categoryIdCategory)
        {
            if (categoryIdCategory == 0)
            {
                return await _context.SubCategories.ToListAsync();
            }

            return await _context.SubCategories.Where(su => su.CategoryId == categoryIdCategory)
                .ToListAsync();
        }
    }
}
