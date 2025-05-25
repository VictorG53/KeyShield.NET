using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class CoffreAcces : ComponentBase
{
    [Parameter] public string Identifiant { get; set; } = string.Empty;

    private bool PasswordOk { get; set; }
    private bool PasswordChecked { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            BooleanResponse? accessResult = await DownstreamApi.GetForUserAsync<BooleanResponse>(
                "KeyShieldAPI",
                options => { options.RelativePath = $"api/coffre/{Identifiant}/access"; }
            );

            if (accessResult is not null && accessResult.Value == true)
            {
                Navigation.NavigateTo($"/coffre/{Identifiant}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking access to coffre: {ex}");
            PasswordOk = false;
            PasswordChecked = true;
            StateHasChanged();
            return;
        }
    }

    private async Task SubmitForm()
    {
        try
        {
            byte[]? hashedPassword = await JS.InvokeAsync<byte[]>("getAndHashPassword");

            if (hashedPassword is null || hashedPassword.Length == 0)
            {
                Console.WriteLine("Hashed password data is empty or null");
                return;
            }

            byte[]? salt = await DownstreamApi.PostForUserAsync<PasswordCheckRequest, byte[]>(
                "KeyShieldAPI",
                new PasswordCheckRequest(
                    hashedPassword
                ),
                options => { options.RelativePath = $"api/coffre/{Identifiant}/checkPassword"; }
            );

            if (salt is not null)
            {
                await JS.InvokeVoidAsync("deriveKey", salt);
                Navigation.NavigateTo($"/coffre/{Identifiant}");
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