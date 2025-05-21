namespace KeyShieldDB.Models;

public class Log
{
    #region Properties
    public Guid Identifiant { get; set; }
    public DateTime Horodatage { get; set; }
    public string Message { get; set; } = null!;
    public Guid UtilisateurCreateurIdentifiant { get; set; }
    public Guid? UtilisateurPartageIdentifiant { get; set; }
    public Guid ActionTypeIdentifiant { get; set; }
    public Guid? CoffreIdentifiant { get; set; }
    public Guid? EntreeIdentifiant { get; set; }
    public Guid? DonneeIdentifiant { get; set; }
    #endregion

    #region Navigation
    public Utilisateur Utilisateur { get; set; } = null!;
    public Utilisateur? UtilisateurPartage { get; set; }
    public ActionType ActionType { get; set; } = null!;
    public Coffre? Coffre { get; set; }
    public Entree? Entree { get; set; }
    public Donnee? Donnee { get; set; }
    #endregion
}