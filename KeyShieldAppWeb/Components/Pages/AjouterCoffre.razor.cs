using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class AjouterCoffre : ComponentBase
{
    private string CoffreNom { get; set; } = string.Empty;

    private async Task SubmitForm()
    {
        try
        {
            byte[] hashedPassword = await JS.InvokeAsync<byte[]>("getAndHashPassword");
            byte[] salt = await JS.InvokeAsync<byte[]>("generateSalt");

            if (salt.Length == 0)
            {
                Console.WriteLine("Salt data is empty or null");
                return;
            }

            if (hashedPassword.Length == 0)
            {
                Console.WriteLine("Hashed password data is empty or null");
                return;
            }

            if (string.IsNullOrWhiteSpace(CoffreNom))
            {
                Console.WriteLine("Coffre name is required");
                return;
            }

            CoffreDTOResponse? response = await DownstreamApi.PostForUserAsync<CoffreDTORequest, CoffreDTOResponse>(
                "KeyShieldAPI",
                new CoffreDTORequest(
                    CoffreNom,
                    salt,
                    hashedPassword,
                    DateTime.Now,
                    Guid.Empty, // sera ignoré côté API
                    false,
                    null
                ),
                options => { options.RelativePath = "api/coffre"; });

            if (response != null)
                // Redirigé vers la page du coffre
                NavigationManager.NavigateTo($"/coffre/{response.Identifiant}");
            else
                Console.WriteLine("API returned null response");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating coffre: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}