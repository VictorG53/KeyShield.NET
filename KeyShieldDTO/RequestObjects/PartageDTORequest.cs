namespace KeyShieldDTO.RequestObjects;

public class PartageDTORequest(Guid Identifiant, Guid UtilisateurIdentifiant, Guid CoffreIdentifiant, DateTime DatePartage, bool EstSupprime, DateTime? DateSuppression)
{

    #region Properties
    public Guid Identifiant { get; set; } = Identifiant;
    public Guid UtilisateurIdentifiant { get; set; } = UtilisateurIdentifiant;
    public Guid CoffreIdentifiant { get; set; } = CoffreIdentifiant;
    public DateTime DatePartage { get; set; } = DatePartage;
    public bool EstSupprime { get; set; } = EstSupprime;
    public DateTime? DateSuppression { get; set; } = DateSuppression;

    #endregion
}