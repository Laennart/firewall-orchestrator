using FWO.Ui.Data;

using Microsoft.AspNetCore.Components.Web;

namespace FWO.Ui.Services
{
    public class KeyboardInputService
    {
        public bool ShiftPressed { get; set; } = false;
        public bool ControlPressed { get; set; } = false;
    }
}
