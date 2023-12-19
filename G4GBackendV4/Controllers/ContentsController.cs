using G4GBackendV4.Data;
using G4GBackendV4.Dtos;
using G4GBackendV4.Models;
using G4GBackendV4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G4GBackendV4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContentsController : ControllerBase
    {
        private readonly ContentService _contentsService;

        public ContentsController(ContentService contentsService)
        {
            _contentsService = contentsService;
        }

        [HttpGet("GetContents")]
        [AllowAnonymous]
        public IActionResult GetContents(int subcategoryIdSubcategory)
        {
            var contents = _contentsService.GetContents(subcategoryIdSubcategory);
            return Ok(contents);
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var content = await _contentsService.GetById(id);
            return Ok(content);
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Create(PostContentDto content)
        {
            return await _contentsService.Create(content);
        }

        [HttpPut("Update")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Update(UpdateContentDto content)
        {
            return await _contentsService.Update(content);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Admin)]
        public async Task<IActionResult> Delete(long id)
        {
            return await _contentsService.Delete(id);
        }
    }
}