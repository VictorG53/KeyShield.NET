using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Home : ComponentBase
{
    private List<CoffreDTOResponse> _coffres = [];

    private async Task FetchCoffre()
    {
        List<CoffreDTOResponse>? coffres = await DownstreamApi.CallApiForUserAsync<List<CoffreDTOResponse>>("KeyShieldAPI", options =>
        {
            options.HttpMethod = "GET";
            options.RelativePath = "api/coffre";
        });
        _coffres = coffres ?? [];
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchCoffre();
    }

    private async Task OnCoffreDeletedAsync()
    {
        await FetchCoffre();
        StateHasChanged(); // Force the component to re-render
    }
}