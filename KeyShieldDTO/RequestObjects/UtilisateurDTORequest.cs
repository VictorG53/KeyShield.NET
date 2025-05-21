namespace KeyShieldDTO.RequestObjects;

public class UtilisateurDTORequest(Guid Identifiant, string EntraId, bool EstSupprime, DateTime? DateSuppression)
{

    #region Properties
    public Guid Identifiant { get; set; } = Identifiant;
    public string EntraId { get; set; } = EntraId;
    public bool EstSupprime { get; set; } = EstSupprime;
    public DateTime? DateSuppression { get; set; } = DateSuppression;

    #endregion
}