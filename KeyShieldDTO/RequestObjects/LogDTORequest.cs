namespace KeyShieldDTO.RequestObjects;

public class LogDTORequest(string message, Guid utilisateurCreateurIdentifiant, Guid actionTypeIdentifiant)
{
    public Guid Identifiant { get; set; } = new Guid();

    public DateTime HoroDatage { get; set; } = DateTime.Now;

    public string Message { get; set; } = message;

    public Guid UtilisateurCreateurIdentifiant { get; set; } = utilisateurCreateurIdentifiant;

    public Guid ActionTypeIdentifiant { get; set; } = actionTypeIdentifiant;
}