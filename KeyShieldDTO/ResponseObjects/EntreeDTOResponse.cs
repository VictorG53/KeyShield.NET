namespace KeyShieldDTO.ResponseObjects;

public class EntreeDTOResponse(
    Guid identifiant,
    Guid coffreIdentifiant,
    DonneeDTOResponse nom,
    DonneeDTOResponse nomUtilisateur,
    DonneeDTOResponse motDePasse,
    DonneeDTOResponse commentaire,
    DonneeDTOResponse dateCreation,
    DonneeDTOResponse dateModification
    )
{
    public Guid Identifiant { get; set; } = identifiant;
    public Guid CoffreIdentifiant { get; set; } = coffreIdentifiant;
    public DonneeDTOResponse Nom { get; set; } = nom;
    public DonneeDTOResponse NomUtilisateur { get; set; } = nomUtilisateur;
    public DonneeDTOResponse MotDePasse { get; set; } = motDePasse;
    public DonneeDTOResponse Commentaire { get; set; } = commentaire;
    public DonneeDTOResponse DateCreation { get; set; } = dateCreation;
    public DonneeDTOResponse DateModification { get; set; } = dateModification;
}