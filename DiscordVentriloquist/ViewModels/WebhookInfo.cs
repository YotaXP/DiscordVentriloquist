using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordVentriloquist.ViewModels
{
    public class WebhookInfo : BindableBase
    {
        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private Uri url;
        public Uri URL {
            get => url;
            set => SetProperty(ref url, value);
        }
    }

    public class WebhookInfoDTI : WebhookInfo
    {
        public WebhookInfoDTI()
        {
            Name = "Sample Webhook";
            URL = new Uri("http://localhost/");
        }
    }
}
