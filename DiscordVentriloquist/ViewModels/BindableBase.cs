using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiscordVentriloquist.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string property = "")
        {
            if (Equals(oldValue, newValue)) return false;
            oldValue = newValue;
            OnPropertyChanged(property);
            return true;
        }

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
