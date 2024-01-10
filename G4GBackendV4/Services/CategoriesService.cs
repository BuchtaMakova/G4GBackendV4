using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G4GBackendV4.Services
{
    public class CategoriesService
    {
        private readonly G4GDbContext _context;

        public CategoriesService(G4GDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.Include(x => x.SubCategories).ToListAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                IdCategory = c.Id,
                Name = c.Name,
                SubCategory = c.SubCategories.Select(sc => CreateSubCategoryDto(sc)).ToList()
            }).ToList();
            return categoriesDto;
        }

        public SubCategoryDto CreateSubCategoryDto(SubCategory sc)
        {
            var lastContent = GetLastContentInSubCategory((int)sc.Id);
            var totalComments = CalculateTotalComments((int)sc.Id);

            return new SubCategoryDto
            {
                Icon = sc.Icon,
                IdSubcategory = sc.Id,
                Name = sc.Name,
                LastContentInSubCategory = lastContent,
                TotalCommentInInSubCategory = totalComments,
                TotalContentsInSubCategory = _context.Contents.Count(cm => cm.SubcategoryId == sc.Id)
            };
        }

        public int CalculateTotalComments(int subcategoryId)
        {
            var totalComments = _context.Contents.Count(cn => cn.SubcategoryId == subcategoryId);
            return totalComments;
        }

        public ContentDto GetLastContentInSubCategory(int subcategoryId)
        {
            return _context.Contents
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

        public async Task<Category> CreateCategoryAsync(string name)
        {
            var category = new Category { Name = name };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}