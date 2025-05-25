using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Pages;

public record EncryptReturn(byte[] Cipher, byte[] Iv, byte[] AuthTag);

public partial class AjouterEntree
{
    [Parameter] public required string Identifiant { get; set; }
    
    private string MotDePasse { get; set; } = string.Empty;
    private bool InclureMajuscules { get; set; } = true;
    private bool InclureSpeciaux { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            BooleanResponse? accessResult = await DownstreamApi.GetForUserAsync<BooleanResponse>(
                "KeyShieldAPI",
                options => { options.RelativePath = $"api/coffre/{Identifiant}/access"; }
            );

            if (accessResult is not null && !accessResult.Value)
            {
                Navigation.NavigateTo("/");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            Navigation.NavigateTo("/");
        }
    }

    private async Task SaveData()
    {
        try
        {

            // Récupération du hash de toutes les entrées
            EncryptReturn nom = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataNom"]);
            EncryptReturn nomUtilisateur = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataNomUtilisateur"]);
            EncryptReturn motDePasse = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataMotDePasse"]);
            EncryptReturn commentaire = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataCommentaire"]);
            EncryptReturn dateCreation = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataDateCreation"]);
            EncryptReturn dateModification = await JS.InvokeAsync<EncryptReturn>("encryptInput", ["dataDateCreation"]);

            Guid coffreId = Guid.TryParse(Identifiant, out Guid id) ? id : Guid.Empty;

            EntreeCreationDTORequest body = new(
                coffreId,
                new DonneeCreationDTORequest(nom.Cipher, nom.Iv, nom.AuthTag),
                new DonneeCreationDTORequest(nomUtilisateur.Cipher, nomUtilisateur.Iv, nomUtilisateur.AuthTag),
                new DonneeCreationDTORequest(motDePasse.Cipher, motDePasse.Iv, motDePasse.AuthTag),
                new DonneeCreationDTORequest(commentaire.Cipher, commentaire.Iv, commentaire.AuthTag),
                new DonneeCreationDTORequest(dateCreation.Cipher, dateCreation.Iv, dateCreation.AuthTag),
                new DonneeCreationDTORequest(dateModification.Cipher, dateModification.Iv, dateModification.AuthTag)
            );

            BooleanResponse? response = await DownstreamApi.PostForUserAsync<EntreeCreationDTORequest, BooleanResponse>(
                "KeyShieldAPI",
                body,
                options => options.RelativePath = "/api/entree"
            );
            Console.WriteLine($"Response: {response}");

            if (response is not null && response.Value)
            {
                Navigation.NavigateTo($"/coffre/{Identifiant}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during the sending of the request : {ex}");
            return;
        }

    }

    private void GoBack()
    {
        Navigation.NavigateTo($"/coffre/{Identifiant}");
    }
}