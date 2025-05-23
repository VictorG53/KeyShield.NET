using KeyShieldDB.Context;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Repositories;

public class EntreeRepository(KeyShieldDbContext dbContext)
{

    public async Task CreateEntreeAsync(EntreeCreationDTORequest request)
    {

        using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            // Création des entités Donnee
            Donnee donneeNom = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.Nom.Cypher,
                IV = request.Nom.IV,
                Tag = request.Nom.Tag
            };
            Donnee donneeNomUtilisateur = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.NomUtilisateur.Cypher,
                IV = request.NomUtilisateur.IV,
                Tag = request.NomUtilisateur.Tag
            };
            Donnee donneeMotDePasse = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.MotDePasse.Cypher,
                IV = request.MotDePasse.IV,
                Tag = request.MotDePasse.Tag
            };
            Donnee donneeCommentaire = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.Commentaire.Cypher,
                IV = request.Commentaire.IV,
                Tag = request.Commentaire.Tag
            };
            Donnee donneeDateCreation = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.DateCreation.Cypher,
                IV = request.DateCreation.IV,
                Tag = request.DateCreation.Tag
            };
            Donnee donneeDateModification = new Donnee
            {
                Identifiant = Guid.NewGuid(),
                Cypher = request.DateModification.Cypher,
                IV = request.DateModification.IV,
                Tag = request.DateModification.Tag
            };

            // Ajout en une seule fois
            dbContext.Donnees.AddRange(
                donneeNom,
                donneeNomUtilisateur,
                donneeMotDePasse,
                donneeCommentaire,
                donneeDateCreation,
                donneeDateModification
            );

            Entree entree = new Entree
            {
                Identifiant = Guid.NewGuid(),
                CoffreIdentifiant = request.CoffreId,
                Nom = donneeNom,
                NomUtilisateur = donneeNomUtilisateur,
                MotDePasse = donneeMotDePasse,
                Commentaire = donneeCommentaire,
                DateCreation = donneeDateCreation,
                DateModification = donneeDateModification
            };
            dbContext.Entrees.Add(entree);

            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<EntreeDTOResponse>> GetAllCoffreEntreesAsync(Guid coffreIdentifiant)
    {
        var entreeList = await dbContext.Entrees
            .Where(e => e.CoffreIdentifiant == coffreIdentifiant)
            .Select(e => new EntreeDTOResponse(
                e.Identifiant,
                e.CoffreIdentifiant,
                e.MotDePasse.Identifiant,
                new DonneeDTOResponse(e.Nom.Identifiant, e.Nom.Cypher, e.Nom.IV, e.Nom.Tag),
                new DonneeDTOResponse(e.NomUtilisateur.Identifiant, e.NomUtilisateur.Cypher, e.NomUtilisateur.IV, e.NomUtilisateur.Tag),
                new DonneeDTOResponse(e.Commentaire.Identifiant, e.Commentaire.Cypher, e.Commentaire.IV, e.Commentaire.Tag),
                new DonneeDTOResponse(e.DateCreation.Identifiant, e.DateCreation.Cypher, e.DateCreation.IV, e.DateCreation.Tag),
                new DonneeDTOResponse(e.DateModification.Identifiant, e.DateModification.Cypher, e.DateModification.IV, e.DateModification.Tag)
            ))
            .ToListAsync();

        return entreeList;
    }

    public async Task<DonneeDTOResponse> GetMotDePasseAsync(Guid donneeIdentifiant)
    {
        DonneeDTOResponse? entree = await dbContext.Donnees
            .Where(e => e.Identifiant == donneeIdentifiant)
            .Select(e => new DonneeDTOResponse(
                e.Identifiant,
                e.Cypher,
                e.IV,
                e.Tag
            ))
            .FirstOrDefaultAsync();

        return entree ?? throw new KeyNotFoundException($"Entree with ID {donneeIdentifiant} not found.");
    }

    public async Task<Guid> GetCoffreIdFromDonneeIdAsync(Guid donneeIdentifiant)
    {
        Guid coffreGuid = await dbContext.Entrees
            .Where(e => e.MotDePasseIdentifiant == donneeIdentifiant)
            .Select(e => e.CoffreIdentifiant)
            .FirstOrDefaultAsync();
        if (coffreGuid == Guid.Empty)
        {
            throw new KeyNotFoundException($"Coffre with Donnee ID {donneeIdentifiant} not found.");
        }
        return coffreGuid;
    }

}