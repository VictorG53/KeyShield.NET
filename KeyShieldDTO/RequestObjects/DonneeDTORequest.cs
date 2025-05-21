namespace KeyShieldDTO.RequestObjects;

public class DonneeDTORequest(Guid identifiant, byte[] cypher, byte[] iv, byte[] tag, bool estSupprime, DateTime? dateSuppression)
{
    #region Properties

    public Guid Identifiant { get; set; } = identifiant;
    public byte[] Cypher { get; set; } = cypher;
    public byte[] IV { get; set; } = iv;
    public byte[] Tag { get; set; } = tag;
    public bool EstSupprime { get; set; } = estSupprime;
    public DateTime? DateSuppression { get; set; } = dateSuppression;

    #endregion
}
