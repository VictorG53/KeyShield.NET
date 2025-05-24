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
    [HttpGet]
    public async Task<List<CoffreDTOResponse>> GetAllCoffresAsync()
    {
        List<CoffreDTOResponse> coffres = await coffreService.GetAllUtilisateurCoffresAsync();
        return coffres;
    }

    [HttpPost]
    public async Task<ActionResult<CoffreDTOResponse>> CreateCoffre([FromBody] CoffreDTORequest request)
    {
        CoffreDTOResponse coffre = await coffreService.CreateCoffreAsync(request);
        return Ok(coffre);
    }

    [HttpGet("{coffreId}/access")]
    public ActionResult<BooleanResponse> IsCoffreUnlocked(string coffreId)
    {
        try
        {
            bool result = coffreService.IsCoffreUnlocked(coffreId);
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
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }

    [HttpPost("{coffreId}/checkPassword")]
    public async Task<ActionResult<byte[]?>> CheckPassword(string coffreId,
        [FromBody] PasswordCheckRequest request)
    {
        try
        {
            byte[]? result = await coffreService.CheckPasswordAsync(coffreId, request.PasswordHash);

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
            bool result = await coffreService.DeleteCoffreAsync(coffreId);
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