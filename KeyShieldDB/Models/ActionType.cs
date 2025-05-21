namespace KeyShieldDB.Models;

public class ActionType
{
    #region Properties

    public Guid Identifiant { get; set; }
    public string Libelle { get; set; } = null!;

    #endregion

    #region Navigation
    public ICollection<Log>? Logs { get; set; }
    #endregion
}