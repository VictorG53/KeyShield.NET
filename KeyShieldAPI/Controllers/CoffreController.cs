using KeyShieldAPI.Services;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShieldAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/coffre")]
public class CoffreController(CoffreService coffreService)
    : ControllerBase
{
    public async Task<List<CoffreDTOResponse>> GetAllCoffresAsync()
    {
        var coffres = await coffreService.GetAllUtilisateurCoffresAsync();
        return coffres;
    }


    [HttpGet("/{coffreId}")]
    public async Task<bool> GetAccess(string coffreId)
    {
        var canAccess = await coffreService.GetAccess(coffreId);
        return true;
    }

    [HttpPost]
    public async Task<ActionResult<CoffreDTOResponse>> CreateCoffre([FromBody] CoffreDTORequest request)
    {
        var coffre = await coffreService.CreateCoffreAsync(request);
        return Ok(coffre);
    }

    [HttpPost("{coffreId}/checkPassword")]
    public async Task<ActionResult<byte[]?>> CheckPassword(string coffreId,
        [FromBody] PasswordCheckRequest request)
    {
        try
        {
            var result = await coffreService.CheckPasswordAsync(coffreId, request.PasswordHash);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }

    [HttpDelete("{coffreId}")]
    public async Task<ActionResult<BooleanResponse>> DeleteCoffre(string coffreId)
    {
        try
        {
            var result = await coffreService.DeleteCoffreAsync(coffreId);
            return Ok(new BooleanResponse(result));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, $"You don't have permission to delete this coffre : {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }
}