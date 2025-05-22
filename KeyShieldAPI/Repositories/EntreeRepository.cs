using System.Diagnostics;
using KeyShieldDB.Context;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;

namespace KeyShieldAPI.Repositories;

public class EntreeRepository(KeyShieldDbContext dbContext)
{

    public async Task CreateEntreeAsync(EntreeCreationDTORequest request)
    {
        var stopwatch = Stopwatch.StartNew();

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

            stopwatch.Stop();
            Console.WriteLine($"Temps création et ajout Donnees: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

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

            stopwatch.Stop();
            Console.WriteLine($"Temps création et ajout Entree: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            await dbContext.SaveChangesAsync();

            stopwatch.Stop();
            Console.WriteLine($"Temps SaveChangesAsync: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}