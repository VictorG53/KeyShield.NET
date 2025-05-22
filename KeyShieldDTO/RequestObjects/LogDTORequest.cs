namespace KeyShieldDTO.RequestObjects;

public class LogDTORequest
{
    public Guid Identifiant { get; set; }
    
    public DateTime HoroDatage { get; set; }

    public string Message { get; set; } = null!;
    
    public Guid UtilisateurCreateurIdentifiant { get; set; }
    
    public Guid ActionTypeIdentifiant { get; set; }
}