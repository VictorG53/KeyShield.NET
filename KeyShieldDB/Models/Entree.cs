using KeyShieldDB.Interfaces;

namespace KeyShieldDB.Models;

public class Entree : ISoftDelete
{
    #region Properties

    public Guid Identifiant { get; set; }
    public Guid CoffreIdentifiant { get; set; }
    public Guid? NomIdentifiant { get; set; }
    public Guid? NomUtilisateurIdentifiant { get; set; }
    public Guid? MotDePasseIdentifiant { get; set; }
    public Guid? CommentaireIdentifiant { get; set; }
    public Guid? DateCreationIdentifiant { get; set; }
    public Guid? DateModificationIdentifiant { get; set; }
    public DateTime? DateSuppression { get; set; }

    #endregion

    #region Navigation

    public Coffre Coffre { get; set; } = null!;
    public Donnee Nom { get; set; } = null!;
    public Donnee NomUtilisateur { get; set; } = null!;
    public Donnee MotDePasse { get; set; } = null!;
    public Donnee Commentaire { get; set; } = null!;

    public Donnee DateCreation { get; set; } = null!;
    public Donnee DateModification { get; set; } = null!;

    #endregion
}