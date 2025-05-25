using KeyShieldDB.Models;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace KeyShieldDB.Context;

public class KeyShieldDbContext : DbContext
{
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Coffre> Coffres { get; set; }
    public DbSet<Entree> Entrees { get; set; }
    public DbSet<Donnee> Donnees { get; set; }
    public DbSet<Partage> Partages { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<ActionType> ActionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=localhost,1433;Database=KeyShieldDb;Trusted_Connection=True;TrustServerCertificate=True;Integrated security=False;User=sa;Password=MyP@ssword1234!");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table names
        modelBuilder.Entity<Utilisateur>().ToTable("Utilisateur");
        modelBuilder.Entity<Coffre>().ToTable("Coffre");
        modelBuilder.Entity<Entree>().ToTable("Entree");
        modelBuilder.Entity<Donnee>().ToTable("Donnee");
        modelBuilder.Entity<Partage>().ToTable("Partage");
        modelBuilder.Entity<Log>().ToTable("Log");
        modelBuilder.Entity<ActionType>().ToTable("ActionType");

        // Soft delete filters
        modelBuilder.Entity<Utilisateur>().HasQueryFilter(u => !u.DateSuppression.HasValue);
        modelBuilder.Entity<Coffre>().HasQueryFilter(u => !u.DateSuppression.HasValue);
        modelBuilder.Entity<Entree>().HasQueryFilter(u => !u.DateSuppression.HasValue);
        modelBuilder.Entity<Donnee>().HasQueryFilter(u => !u.DateSuppression.HasValue);
        modelBuilder.Entity<Partage>().HasQueryFilter(u => !u.DateSuppression.HasValue);

        // Utilisateur
        modelBuilder.Entity<Utilisateur>()
            .HasKey(u => u.Identifiant);
        modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.EntraId)
            .IsUnique();
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.EntraId)
            .IsRequired();

        // Coffre
        modelBuilder.Entity<Coffre>()
            .HasKey(c => c.Identifiant);
        modelBuilder.Entity<Coffre>()
            .Property(c => c.Nom)
            .IsRequired();
        modelBuilder.Entity<Coffre>()
            .Property(c => c.Sel)
            .IsRequired();
        modelBuilder.Entity<Coffre>()
            .Property(c => c.MotDePasseHash)
            .IsRequired();
        modelBuilder.Entity<Coffre>()
            .Property(c => c.DateCreation)
            .IsRequired();
        modelBuilder.Entity<Coffre>()
            .HasOne(c => c.Utilisateur)
            .WithMany(u => u.Coffres)
            .HasForeignKey(c => c.UtilisateurIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);

        // Entree
        modelBuilder.Entity<Entree>()
            .HasKey(e => e.Identifiant);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.Coffre)
            .WithMany(c => c.Entrees)
            .HasForeignKey(e => e.CoffreIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.Nom)
            .WithMany()
            .HasForeignKey(e => e.NomIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.NomUtilisateur)
            .WithMany()
            .HasForeignKey(e => e.NomUtilisateurIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.MotDePasse)
            .WithMany()
            .HasForeignKey(e => e.MotDePasseIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.Commentaire)
            .WithMany()
            .HasForeignKey(e => e.CommentaireIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.DateCreation)
            .WithMany()
            .HasForeignKey(e => e.DateCreationIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Entree>()
            .HasOne(e => e.DateModification)
            .WithMany()
            .HasForeignKey(e => e.DateModificationIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);

        // Donnee
        modelBuilder.Entity<Donnee>()
            .HasKey(d => d.Identifiant);
        modelBuilder.Entity<Donnee>()
            .Property(d => d.Cypher)
            .IsRequired();
        modelBuilder.Entity<Donnee>()
            .Property(d => d.IV)
            .IsRequired();
        modelBuilder.Entity<Donnee>()
            .Property(d => d.Tag)
            .IsRequired();

        // Partage
        modelBuilder.Entity<Partage>()
            .HasKey(p => p.Identifiant);
        modelBuilder.Entity<Partage>()
            .HasOne(p => p.Utilisateur)
            .WithMany(u => u.Partages)
            .HasForeignKey(p => p.UtilisateurIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Partage>()
            .HasOne(p => p.Coffre)
            .WithMany(c => c.Partages)
            .HasForeignKey(p => p.CoffreIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);

        // Log
        modelBuilder.Entity<Log>()
            .HasKey(l => l.Identifiant);
        modelBuilder.Entity<Log>()
            .Property(l => l.Message)
            .IsRequired();
        modelBuilder.Entity<Log>()
            .HasOne(l => l.Utilisateur)
            .WithMany()
            .HasForeignKey(l => l.UtilisateurCreateurIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Log>()
            .HasOne(l => l.UtilisateurPartage)
            .WithMany()
            .HasForeignKey(l => l.UtilisateurPartageIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Log>()
            .HasOne(l => l.ActionType)
            .WithMany(a => a.Logs)
            .HasForeignKey(l => l.ActionTypeIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Log>()
            .HasOne(l => l.Coffre)
            .WithMany()
            .HasForeignKey(l => l.CoffreIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Log>()
            .HasOne(l => l.Entree)
            .WithMany()
            .HasForeignKey(l => l.EntreeIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Log>()
            .HasOne(l => l.Donnee)
            .WithMany()
            .HasForeignKey(l => l.DonneeIdentifiant)
            .OnDelete(DeleteBehavior.NoAction);

        // ActionType
        modelBuilder.Entity<ActionType>()
            .HasKey(a => a.Identifiant);
        modelBuilder.Entity<ActionType>()
            .Property(a => a.Libelle)
            .IsRequired();
    }
}