using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KeyShieldAPI.Services;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;

namespace KeyShieldAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/entree")]
public class EntreeController(EntreeService entreeService) : ControllerBase
{
    [HttpPost()]
    public async Task<ActionResult<BooleanResponse>> CreateEntreeAsync([FromBody] EntreeCreationDTORequest request)
    {
        try
        {
            await entreeService.CreateEntreeAsync(request);
            return Ok(new BooleanResponse(true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, $"An error occurred while processing your request: {ex}");
        }
    }

}