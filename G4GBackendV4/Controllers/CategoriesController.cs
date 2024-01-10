using G4GBackendV4.Services;
using G4GBackendV4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet("GetCategories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories()
    {
        var categoriesDto = await _categoriesService.GetCategoriesAsync();
        return Ok(categoriesDto);
    }

    [HttpPost("Create")]
    [Authorize(Roles = CustomRoles.Admin)]
    public async Task<IActionResult> Create(string name)
    {
        var category = await _categoriesService.CreateCategoryAsync(name);
        return Ok(category);
    }
}