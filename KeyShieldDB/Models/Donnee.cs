namespace KeyShieldDB.Models;

public class Donnee : Interfaces.ISoftDelete
{
    #region Properties

    public Guid Identifiant { get; set; }
    public byte[] Cypher { get; set; } = null!;
    public byte[] IV { get; set; } = null!;
    public byte[] Tag { get; set; } = null!;
    public DateTime? DateSuppression { get; set; }

    #endregion
}