namespace KeyShieldDB.Models;

public class Coffre : Interfaces.ISoftDelete
{
    #region Properties

    public Guid Identifiant { get; set; }
    public string Nom { get; set; } = null!;
    public byte[] Sel { get; set; } = null!;
    public byte[] MotDePasseHash { get; set; } = null!;
    public DateTime DateCreation { get; set; }
    public Guid UtilisateurIdentifiant { get; set; }
    public DateTime? DateSuppression { get; set; }

    #endregion

    #region Navigation

    public Utilisateur Utilisateur { get; set; } = null!;
    public ICollection<Entree>? Entrees { get; set; }
    public ICollection<Partage>? Partages { get; set; }

    #endregion
}