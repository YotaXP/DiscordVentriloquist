﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SampleDataSource
{
    using System; 
    using System.ComponentModel;

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
    internal class SampleDataSource { }
#else

    public class SampleDataSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SampleDataSource()
        {
            try
            {
                Uri resourceUri = new Uri("/DiscordVentriloquist;component/SampleData/SampleDataSource/SampleDataSource.xaml", UriKind.RelativeOrAbsolute);
                System.Windows.Application.LoadComponent(this, resourceUri);
            }
            catch
            {
            }
        }

        private Webhooks _Webhooks = new Webhooks();

        public Webhooks Webhooks
        {
            get
            {
                return this._Webhooks;
            }
        }

        private Characters _Characters = new Characters();

        public Characters Characters
        {
            get
            {
                return this._Characters;
            }
        }

        private string _Message = string.Empty;

        public string Message
        {
            get
            {
                return this._Message;
            }

            set
            {
                if (this._Message != value)
                {
                    this._Message = value;
                    this.OnPropertyChanged("Message");
                }
            }
        }
    }

    public class WebhooksItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Name = string.Empty;

        public string Name
        {
            get
            {
                return this._Name;
            }

            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        private string _URL = string.Empty;

        public string URL
        {
            get
            {
                return this._URL;
            }

            set
            {
                if (this._URL != value)
                {
                    this._URL = value;
                    this.OnPropertyChanged("URL");
                }
            }
        }
    }

    public class Webhooks : System.Collections.ObjectModel.ObservableCollection<WebhooksItem>
    { 
    }

    public class CharactersItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Name = string.Empty;

        public string Name
        {
            get
            {
                return this._Name;
            }

            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        private string _AvatarURL = string.Empty;

        public string AvatarURL
        {
            get
            {
                return this._AvatarURL;
            }

            set
            {
                if (this._AvatarURL != value)
                {
                    this._AvatarURL = value;
                    this.OnPropertyChanged("AvatarURL");
                }
            }
        }
    }

    public class Characters : System.Collections.ObjectModel.ObservableCollection<CharactersItem>
    { 
    }
#endif
}
