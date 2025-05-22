using KeyShieldDTO.RequestObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Coffre : ComponentBase
{
    [Parameter] public string Identifiant { get; set; } = string.Empty;

    public byte[]? Sel { get; set; }

    private bool PasswordOk { get; set; }
    private bool PasswordChecked { get; set; }

    private async Task SubmitForm()
    {
        try
        {
            var hashedPassword = await JS.InvokeAsync<byte[]?>("getAndHashPassword");

            if (hashedPassword is null || hashedPassword.Length == 0)
            {
                Console.WriteLine("Hashed password data is empty or null");
                return;
            }

            var salt = await DownstreamApi.PostForUserAsync<PasswordCheckRequest, byte[]>(
                "KeyShieldAPI",
                new PasswordCheckRequest(
                    hashedPassword
                ),
                options => { options.RelativePath = $"api/coffre/{Identifiant}/checkPassword"; }
            );

            if (salt is not null)
            {
                await JS.InvokeVoidAsync("deriveKey", salt);
                Sel = salt;
                PasswordOk = true;
            }

            PasswordChecked = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing coffre : {ex.Message}");
            PasswordOk = false;
            PasswordChecked = true;
            StateHasChanged();
        }
    }
}