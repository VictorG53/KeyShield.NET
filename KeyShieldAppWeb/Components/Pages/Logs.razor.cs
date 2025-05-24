using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Components;

namespace KeyShieldAppWeb.Components.Pages;

public partial class Logs : ComponentBase
{
    private List<LogDTOResponse> _logs = [];

    private async Task FetchLogs()
    {
        var logs = await DownstreamApi.GetForUserAsync<List<LogDTOResponse>>("KeyShieldAPI", options =>
        {
            options.RelativePath = "api/log";
        });
        _logs = logs ?? [];
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchLogs();
    }
}