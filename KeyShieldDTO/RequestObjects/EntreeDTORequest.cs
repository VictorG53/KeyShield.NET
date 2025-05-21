namespace KeyShieldDTO.RequestObjects;

public class EntreeDTORequest(
    Guid identifiant,
    Guid coffreIdentifiant,
    Guid? nomIdentifiant,
    Guid? nomUtilisateurIdentifiant,
    Guid? motDePasseIdentifiant,
    Guid? commentaireIdentifiant,
    Guid? dateCreationIdentifiant,
    Guid? dateModificationIdentifiant,
    bool estSupprime,
    DateTime? dateSuppression)
{
    public Guid Identifiant { get; set; } = identifiant;
    public Guid CoffreIdentifiant { get; set; } = coffreIdentifiant;
    public Guid? NomIdentifiant { get; set; } = nomIdentifiant;
    public Guid? NomUtilisateurIdentifiant { get; set; } = nomUtilisateurIdentifiant;
    public Guid? MotDePasseIdentifiant { get; set; } = motDePasseIdentifiant;
    public Guid? CommentaireIdentifiant { get; set; } = commentaireIdentifiant;
    public Guid? DateCréationIdentifiant { get; set; } = dateCreationIdentifiant;
    public Guid? DateModificationIdentifiant { get; set; } = dateModificationIdentifiant;
    public bool EstSupprime { get; set; } = estSupprime;
    public DateTime? DateSuppression { get; set; } = dateSuppression;
}