using FWO.Config.Api;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FWO.Ui.Pages.Help
{
    public class MainModel : PageModel
    {
        public UserConfig userConfig { get; set; }

        public MainModel(UserConfig userConfig)
        {
            this.userConfig = userConfig;
        }

        public void OnGet(string lang)
        {
            userConfig.SetLanguage(lang);
        }
    }
}
