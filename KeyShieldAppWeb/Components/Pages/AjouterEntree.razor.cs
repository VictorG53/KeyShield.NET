using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Pages;

public record EncryptReturn(byte[] CipherData, byte[] IV, byte[] AuthTag);

public partial class AjouterEntree
{
    [Parameter] public required string Identifiant { get; set; }

    private async Task SaveData()
    {
        // Récupération du hash de toutes les entrées
        var nom = await JS.InvokeAsync<EncryptReturn>("encrypt", ["dataNom"]);
        var nomUtilisateur = await JS.InvokeAsync<EncryptReturn>("encrypt", ["dataNomUtilisateur"]);
        var motDePasse = await JS.InvokeAsync<EncryptReturn>("encrypt", ["dataMotDePasse"]);
        var commentaire = await JS.InvokeAsync<EncryptReturn>("encrypt", ["dataCommentaire"]);


        Navigation.NavigateTo($"/coffre/{Identifiant}");
    }

    private void GoBack()
    {
        Navigation.NavigateTo($"/coffre/{Identifiant}");
    }
}