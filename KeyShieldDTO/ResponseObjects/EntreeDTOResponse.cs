namespace KeyShieldDTO.ResponseObjects;

public class EntreeDTOResponse(
    Guid identifiant,
    Guid coffreIdentifiant,
    Guid motDePasseIdentifiant,
    DonneeDTOResponse nom,
    DonneeDTOResponse nomUtilisateur,
    DonneeDTOResponse commentaire,
    DonneeDTOResponse dateCreation,
    DonneeDTOResponse dateModification
    )
{
    public Guid Identifiant { get; set; } = identifiant;
    public Guid CoffreIdentifiant { get; set; } = coffreIdentifiant;
    public Guid MotDePasseIdentifiant { get; set; } = motDePasseIdentifiant;
    public DonneeDTOResponse Nom { get; set; } = nom;
    public DonneeDTOResponse NomUtilisateur { get; set; } = nomUtilisateur;
    public DonneeDTOResponse Commentaire { get; set; } = commentaire;
    public DonneeDTOResponse DateCreation { get; set; } = dateCreation;
    public DonneeDTOResponse DateModification { get; set; } = dateModification;
}