namespace KeyShieldDB.Models;

public class Utilisateur : Interfaces.ISoftDelete
{
    #region Properties

    public Guid Identifiant { get; set; }
    public string EntraId { get; set; } = null!;
    public DateTime? DateSuppression { get; set; }

    #endregion

    #region Navigation

    public ICollection<Coffre>? Coffres { get; set; }
    public ICollection<Partage>? Partages { get; set; }

    #endregion
}
