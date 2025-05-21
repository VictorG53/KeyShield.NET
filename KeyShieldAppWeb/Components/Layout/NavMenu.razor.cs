using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Layout;

public partial class NavMenu : ComponentBase
{
    private List<CoffreDTOResponse> _coffres = [];

    protected override async Task OnInitializedAsync()
    {
        var coffres = await DownstreamApi.CallApiForUserAsync<List<CoffreDTOResponse>>("KeyShieldAPI", options =>
        {
            options.HttpMethod = "GET";
            options.RelativePath = "api/coffre";
        });
        _coffres = coffres ?? [];
    }
}