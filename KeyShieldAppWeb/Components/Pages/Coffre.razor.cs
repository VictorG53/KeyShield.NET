using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Coffre : ComponentBase
{
    [Parameter] public string Identifiant { get; set; } = string.Empty;

    public byte[]? Sel { get; set; }

    private DotNetObjectReference<Coffre>? dotNetRef;

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
                if (accessResult.Value)
                {
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
            Console.WriteLine($"Error accessing coffre: {ex}");
            Navigation.NavigateTo($"/coffre/{Identifiant}/access");
        }

        dotNetRef = DotNetObjectReference.Create(this);
        await JS.InvokeVoidAsync("setCoffreDotNetRef", dotNetRef);
    }

    [JSInvokable]
    public async Task GetPassword(string entreeId)
    {
        Console.WriteLine($"Getting password for entreeId: {entreeId}");
        DonneeDTOResponse? entree = await DownstreamApi.GetForUserAsync<DonneeDTOResponse>(
            "KeyShieldAPI",
            options => { options.RelativePath = $"api/entree/motDePasse/{entreeId}"; }
        );
        await JS.InvokeVoidAsync("getPassword", [entree]);
    }

    public void Dispose()
    {
        dotNetRef?.Dispose();
    }
}