using G4GBackendV4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G4GBackendV4.Controllers;

public class RoleController : Controller
{
    private readonly RoleService? _roleService;

    public RoleController(RoleService? roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("CreateRole")]
    [Authorize(Roles = CustomRoles.Admin)]
    public async Task<IActionResult> CreateRole(string name)
    {
        return Ok(await _roleService?.CreateRole(name)!);
    }

    [HttpPost("AddRole")]
    [Authorize(Roles = CustomRoles.Admin)]
    public async Task<IActionResult> AddRole(string username, string role)
    {
        try
        {
            await _roleService?.AddRoleToUser(username, role)!;
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        return Ok();
    }
}