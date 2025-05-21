using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Pages;

public partial class CoffreAdd
{
    [Parameter] public string Identifiant { get; set; }
    private string NomEntree { get; set; }
    private string Nom { get; set; }
    private string MotDePasse { get; set; }
    private string Commentaire { get; set; }

    private async Task SaveData()
    {
        // Here you would implement actual data saving logic
        // For example, calling an API through DownstreamApi

        // After saving, navigate back to the coffre page
        Navigation.NavigateTo($"/coffre/{Identifiant}");
    }

    private void GoBack()
    {
        Navigation.NavigateTo($"/coffre/{Identifiant}");
    }
}