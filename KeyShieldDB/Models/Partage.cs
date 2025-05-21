namespace KeyShieldDB.Models;

public class Partage : Interfaces.ISoftDelete
{
    #region Properties

    public Guid Identifiant { get; set; }
    public Guid UtilisateurIdentifiant { get; set; }
    public Guid CoffreIdentifiant { get; set; }
    public DateTime DatePartage { get; set; }
    public DateTime? DateSuppression { get; set; }

    #endregion

    #region Navigation

    public Utilisateur Utilisateur { get; set; } = null!;
    public Coffre Coffre { get; set; } = null!;

    #endregion
}