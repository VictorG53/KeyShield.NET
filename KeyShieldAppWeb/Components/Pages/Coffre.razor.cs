using KeyShieldDTO.RequestObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Coffre : ComponentBase
{
    [Parameter] public string Identifiant { get; set; } = string.Empty;

    public byte[]? Sel { get; set; }

    public bool PasswordOk { get; set; }
    public bool PasswordChecked { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await JS.InvokeVoidAsync("initFormSubmission", DotNetObjectReference.Create(this));
    }

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

            var response = await DownstreamApi.PostForUserAsync<PasswordCheckRequest, byte[]>(
                "KeyShieldAPI",
                new PasswordCheckRequest(
                    hashedPassword
                ),
                options => { options.RelativePath = $"api/coffre/{Identifiant}/checkPassword"; }
            );

            if (response is not null) PasswordOk = true;

            Sel = response ?? null;

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