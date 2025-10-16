using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MaintenanceController : ControllerBase
{
    private ICleanupService _cleanupService;

    public MaintenanceController(ICleanupService cleanupService)
    {
        _cleanupService = cleanupService;
    }
    
    [HttpPost]
    [Authorize]
    [Route("Cleanup")]
    public async Task<IActionResult> PostCleanup()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            await _cleanupService.DeleteAllAsync();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok($"Conta removida com sucesso");
    }
}