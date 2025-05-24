using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Components;

public partial class CoffreItem : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public string Id { get; set; } = string.Empty;
    [Parameter] public string Name { get; set; } = string.Empty;
    [Parameter] public EventCallback OnCoffreDeleted { get; set; }

    private void NavigateToCoffre()
    {
        NavigationManager.NavigateTo($"/coffre/{Id}/access");
    }

    private async Task DeleteCoffre()
    {
        try
        {
            BooleanResponse? response = await DownstreamApi.DeleteForUserAsync<object, BooleanResponse>(
                "KeyShieldAPI",
                new object(),
                options => { options.RelativePath = $"api/coffre/{Id}"; }
            );

            if (response != null && response.Value == true) await OnCoffreDeleted.InvokeAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception when deleting coffre: {ex.Message}");
        }
    }
}