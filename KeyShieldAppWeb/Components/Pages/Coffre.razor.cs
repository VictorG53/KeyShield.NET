using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Coffre : ComponentBase
{
    [Parameter] public string Identifiant { get; set; } = string.Empty;

    public byte[]? Sel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            BooleanResponse? accessResult = await DownstreamApi.GetForUserAsync<BooleanResponse>(
                "KeyShieldAPI",
                options => { options.RelativePath = $"api/coffre/{Identifiant}/access"; }
            );


            if (accessResult is not null)
            {
                Console.WriteLine($"Access result is not null");
                if (accessResult.Value)
                {
                    Console.WriteLine($"Access result: {accessResult.Value}");
                    List<EntreeDTOResponse>? entreeList = await DownstreamApi.GetForUserAsync<List<EntreeDTOResponse>>(
                        "KeyShieldAPI",
                        options => { options.RelativePath = $"api/entree/{Identifiant}"; }
                    );

                    await JS.InvokeVoidAsync("decryptAndDisplay", entreeList);
                }
                else
                {
                    Navigation.NavigateTo($"/coffre/{Identifiant}/access");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing coffre");
            Navigation.NavigateTo($"/coffre/{Identifiant}/access");
        }
    }
}