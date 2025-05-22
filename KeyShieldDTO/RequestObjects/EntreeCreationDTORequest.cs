namespace KeyShieldDTO.RequestObjects;

public class EntreeCreationDTORequest(
    Guid coffreId,
    DonneeCreationDTORequest nom,
    DonneeCreationDTORequest nomUtilisateur,
    DonneeCreationDTORequest motDePasse,
    DonneeCreationDTORequest commentaire,
    DonneeCreationDTORequest dateCreation,
    DonneeCreationDTORequest dateModification
)
{
    public Guid CoffreId { get; set; } = coffreId;
    public DonneeCreationDTORequest Nom { get; set; } = nom;
    public DonneeCreationDTORequest NomUtilisateur { get; set; } = nomUtilisateur;
    public DonneeCreationDTORequest MotDePasse { get; set; } = motDePasse;
    public DonneeCreationDTORequest Commentaire { get; set; } = commentaire;
    public DonneeCreationDTORequest DateCreation { get; set; } = dateCreation;
    public DonneeCreationDTORequest DateModification { get; set; } = dateModification;
}