namespace KeyShieldDTO.ResponseObjects;

public class EntreeDTOResponse(
    Guid identifiant,
    Guid coffreIdentifiant,
    Guid? nomIdentifiant,
    Guid? nomUtilisateurIdentifiant,
    Guid? motDePasseIdentifiant,
    Guid? commentaireIdentifiant,
    Guid? dateCreationIdentifiant,
    Guid? dateModificationIdentifiant
)
{
    #region Properties

    public Guid Identifiant { get; set; } = identifiant;
    public Guid CoffreIdentifiant { get; set; } = coffreIdentifiant;
    public Guid? NomIdentifiant { get; set; } = nomIdentifiant;
    public Guid? NomUtilisateurIdentifiant { get; set; } = nomUtilisateurIdentifiant;
    public Guid? MotDePasseIdentifiant { get; set; } = motDePasseIdentifiant;
    public Guid? CommentaireIdentifiant { get; set; } = commentaireIdentifiant;
    public Guid? DateCreationIdentifiant { get; set; } = dateCreationIdentifiant;
    public Guid? DateModificationIdentifiant { get; set; } = dateModificationIdentifiant;

    #endregion
}