namespace KeyShieldDTO.RequestObjects;

public class CoffreDTORequest(string Nom, byte[] Sel, byte[] MotDePasseHash, DateTime DateCreation, Guid UtilisateurIdentifiant, bool EstSupprime, DateTime? DateSuppression)
{

    #region Properties
    public Guid? Identifiant { get; set; }
    public string Nom { get; set; } = Nom;
    public byte[] Sel { get; set; } = Sel;
    public byte[] MotDePasseHash { get; set; } = MotDePasseHash;
    public DateTime DateCreation { get; set; } = DateCreation;
    public Guid UtilisateurIdentifiant { get; set; } = UtilisateurIdentifiant;
    public bool EstSupprime { get; set; } = EstSupprime;
    public DateTime? DateSuppression { get; set; } = DateSuppression;

    #endregion
}