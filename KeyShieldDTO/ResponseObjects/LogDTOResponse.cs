namespace KeyShieldDTO.ResponseObjects;

public class LogDTOResponse
{
    public Guid Identifiant { get; set; }
    
    public Guid UtilisateurCreateurIdentifiant { get; set; }
    
    public string Message { get; set; } = null!;
}