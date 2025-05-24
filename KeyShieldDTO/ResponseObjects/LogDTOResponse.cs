namespace KeyShieldDTO.ResponseObjects;

public class LogDTOResponse
{
    public Guid Identifiant { get; set; }
    
    public DateTime HoroDatage { get; set; }
    
    public string Message { get; set; } = null!;
    
    public Guid UtilisateurCreateurIdentifiant { get; set; }
    
    public string Action { get; set; } = null!;
    
}