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
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }

    [HttpGet("motDePasse/{motDePasseIdentifiant}")]
    public async Task<ActionResult<DonneeDTOResponse>> GetMotDePasseAsync(string motDePasseIdentifiant)
    {
        try
        {
            DonneeDTOResponse result = await entreeService.GetMotDePasseAsync(motDePasseIdentifiant);
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
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }

    [HttpGet("{coffreId}")]
    public async Task<ActionResult<List<EntreeDTOResponse>>> GetEntreesAsync(string coffreId)
    {
        try
        {
            List<EntreeDTOResponse> result = await entreeService.GetAllCoffreEntreesAsync(coffreId);
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
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, $"An error occurred while processing your request : {ex.Message}");
        }
    }

}