using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KeyShieldAppWeb.Components.Components;

public partial class PasswordGenerator : ComponentBase
{
        [Inject] private IJSRuntime JS { get; set; } = default!;

        [Parameter] public string Password { get; set; } = string.Empty;
        [Parameter] public string TargetInputId { get; set; } = "coffrePassword";
        [Parameter] public EventCallback<string> PasswordChanged { get; set; }
        [Parameter] public bool InclureMajuscules { get; set; } = true;
        [Parameter] public EventCallback<bool> InclureMajusculesChanged { get; set; }
        [Parameter] public bool InclureSpeciaux { get; set; } = true;
        [Parameter] public EventCallback<bool> InclureSpeciauxChanged { get; set; }

        private async Task GenererMotDePasse()
        {
            var pwd = await JS.InvokeAsync<string>(
                "generateRandomPassword",
                16,
                InclureMajuscules,
                InclureSpeciaux
            );
            await PasswordChanged.InvokeAsync(pwd);
        }
        
        private async Task UtiliserMotDePasse()
        {
            await PasswordChanged.InvokeAsync(Password);
        }

}