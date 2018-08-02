using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordVentriloquist.ViewModels
{
    public class CharacterInfo : BindableBase
    {
        private string _Name;
        public string Name {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private Uri _AvatarUrl;
        public Uri AvatarUrl {
            get => _AvatarUrl;
            set => SetProperty(ref _AvatarUrl, value);
        }
    }
}
