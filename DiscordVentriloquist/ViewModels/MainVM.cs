using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DiscordVentriloquist.ViewModels
{
    public class MainVM : BindableBase
    {
        public MainVM() {
            //Webhooks.Add(new WebhookInfo() {
            //    Name = "Test",
            //    URL = new Uri("https://webhook.site/428ca1d4-678a-47b6-9c56-405bce06543c")
            //});
            //Webhooks.Add(new WebhookInfo() {
            //    Name = "Discord",
            //    URL = new Uri("https://discordapp.com/api/webhooks/474311499070832646/7oFn0uIET74o7JyrtRuGzpswQ0fDiMF45Nh4DyHTYcPPSyQopiUeUSpdzQ_KUSC8eQvX")
            //});
            //Characters.Add(new CharacterInfo() {
            //    Name = "Vvevv",
            //    AvatarUrl = new Uri("https://cdn.discordapp.com/attachments/122936228877303810/473616229572018186/unknown.png")
            //});

            SendCommand = new RelayCommand(OnSend);
            AddWebhookCommand = new RelayCommand(OnAddWebhook);
            AddCharacterCommand = new RelayCommand(OnAddCharacter);
            RemoveWebhookCommand = new RelayCommand(OnRemoveWebhook);
            RemoveCharacterCommand = new RelayCommand(OnRemoveCharacter);
            SaveToTextCommand = new RelayCommand(OnSaveToText);
            LoadFromTextCommand = new RelayCommand(OnLoadFromText);
        }

        private HttpClient http;

        public ObservableCollection<WebhookInfo> Webhooks { get; private set; } = new ObservableCollection<WebhookInfo>();
        public ObservableCollection<CharacterInfo> Characters { get; private set; } = new ObservableCollection<CharacterInfo>();

        private WebhookInfo _SelectedWebhook;
        public WebhookInfo SelectedWebhook {
            get => _SelectedWebhook;
            set {
                SetProperty(ref _SelectedWebhook, value);
                OnPropertyChanged(nameof(IsWebhookSelected));
            }
        }
        public bool IsWebhookSelected => SelectedWebhook != null;


        private CharacterInfo _SelectedCharacter;
        public CharacterInfo SelectedCharacter {
            get => _SelectedCharacter;
            set {
                SetProperty(ref _SelectedCharacter, value);
                OnPropertyChanged(nameof(IsCharacterSelected));
            }
        }
        public bool IsCharacterSelected => SelectedCharacter != null;

        private string _Message;
        public string Message {
            get => _Message;
            set => SetProperty(ref _Message, value);
        }

        private bool _CanSend = true;
        public bool CanSend {
            get => _CanSend;
            set => SetProperty(ref _CanSend, value);
        }


        private struct WebhookMessage
        {
            public string username;
            public string avatar_url;
            public string content;
        }

        private Task<HttpResponseMessage> SendToWebhook(WebhookInfo webhook, CharacterInfo character, string message) {
            if (http == null) http = new HttpClient();

            var json = JsonConvert.SerializeObject(new WebhookMessage() {
                username = character.Name,
                avatar_url = character.AvatarUrl.ToString(),
                content = message
            });

            return http.PostAsync(webhook.URL, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        private struct SaveObject
        {
            public WebhookInfo[] Webhooks;
            public CharacterInfo[] Characters;
        }

        private readonly byte[] EncryptionKey = new byte[] { 234, 12, 74, 123, 83, 30, 1, 245, 76, 232, 120, 34, 211, 102, 82, 12 };
        public string SaveToText(bool encrypt) {
            var json = JsonConvert.SerializeObject(new SaveObject() {
                Webhooks = Webhooks.ToArray(),
                Characters = Characters.ToArray(),
            });
            if (encrypt) {
                byte[] bytes = UTF8Encoding.UTF8.GetBytes(json);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = EncryptionKey;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                tdes.Clear();

                json = Convert.ToBase64String(resultArray);
            }
            return json;
        }
        public bool LoadFromText(string json, bool encrypted) {
            try {
                if (encrypted) {
                    byte[] bytes = Convert.FromBase64String(json);

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = EncryptionKey;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                    tdes.Clear();

                    json = Encoding.Default.GetString(resultArray);
                }

                var save = JsonConvert.DeserializeObject<SaveObject>(json);

                SelectedWebhook = null;
                Webhooks.Clear();
                foreach (var webhook in save.Webhooks)
                    Webhooks.Add(webhook);

                SelectedCharacter = null;
                Characters.Clear();
                foreach (var character in save.Characters)
                    Characters.Add(character);

                return true;
            }
            catch {
                return false;
            }
        }

        #region Commands

        public ICommand SendCommand { get; private set; }
        private async void OnSend(object sender) {
            if (string.IsNullOrWhiteSpace(Message)) return;
            if (SelectedCharacter == null) {
                MessageBox.Show("Select a character.");
                return;
            }
            if (SelectedWebhook == null) {
                MessageBox.Show("Select a webhook.");
                return;
            }

            string errorMessage = null;
            int code = 0;
            try {
                CanSend = false;

                var response = await SendToWebhook(SelectedWebhook, SelectedCharacter, Message);
                errorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                code = (int)response.StatusCode;
                // 204 NoContent (Response from Discord)
                // 429 Too Many Requests

                if (response.StatusCode == (HttpStatusCode)429) {
                    var wait = int.Parse(response.Headers.GetValues("Retry-After").Single());
                    await Task.Delay(wait);
                }

                if (response.StatusCode == HttpStatusCode.NoContent) {
                    var remaining = int.Parse(response.Headers.GetValues("X-RateLimit-Remaining").Single());
                    if (remaining == 0) {
                        var reset = int.Parse(response.Headers.GetValues("X-RateLimit-Reset").Single());
                        var waitTill = new DateTime(1970, 1, 1).AddMilliseconds(reset);
                        var now = DateTime.Now;
                        if (now < waitTill)
                            await Task.Delay(waitTill - now);
                    }
                }

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                    Message = "";
            }
            catch {
                if (string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show($"An unknown error has ocurred while sending the message.");
                else
                    MessageBox.Show($"An error ocurred.  Response from server: ({code})\n{errorMessage}");
            }
            finally {
                CanSend = true;
            }
        }

        public ICommand RemoveWebhookCommand { get; private set; }
        private void OnRemoveWebhook(object sender) {
            if (SelectedWebhook == null) return;
            Webhooks.Remove(SelectedWebhook);
        }
        public ICommand RemoveCharacterCommand { get; private set; }
        private void OnRemoveCharacter(object sender) {
            if (SelectedCharacter == null) return;
            Characters.Remove(SelectedCharacter);
        }

        public ICommand AddWebhookCommand { get; private set; }
        private void OnAddWebhook(object sender) {
            Webhooks.Add(SelectedWebhook = new WebhookInfo() {
                Name = "New Webhook"
            });
        }
        public ICommand AddCharacterCommand { get; private set; }
        private void OnAddCharacter(object sender) {
            Characters.Add(SelectedCharacter = new CharacterInfo() {
                Name = "New Character",
                AvatarUrl = new Uri("https://i.imgur.com/HC7aBeb.png")
            });
        }

        public ICommand SaveToTextCommand { get; private set; }
        private void OnSaveToText(object sender) {
            TextWindow.ShowDialog(SaveToText(true), "Save to Text", false, true);
        }
        public ICommand LoadFromTextCommand { get; private set; }
        private void OnLoadFromText(object sender) {
            var data = TextWindow.ShowDialog("", "Load from Text", true, false);
            if (string.IsNullOrWhiteSpace(data)) return;
            if (!LoadFromText(data, true))
                MessageBox.Show("Failed to load from the data given.");
        }

        #endregion
    }
}
