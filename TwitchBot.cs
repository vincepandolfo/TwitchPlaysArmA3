using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchPlaysArmA3.Voting;

namespace TwitchPlaysArmA3
{
    public class TwitchBot
    {
        private TwitchClient _twitchClient;
        private int _actionInterval = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
        private ExtensionCallback _callback;
        private IDictionary<string, string> _locale;
        private VotingManager _votingManager = new VotingManager();
        private string _channel;

        private CancellationTokenSource _tokenSource;
        private Thread _runningBot;

        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            _twitchClient.Connect();
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _runningBot.Join();
        }

        public void AddSpawnClass(string name, IEnumerable<string> areas, IEnumerable<string> types)
        {
            _votingManager.AddSpawnClass(name, areas, types);
        }

        public void SetCallback(ExtensionCallback callback)
        {
            _votingManager.SetCallback(callback);
            _callback = callback;
        }

        public void SetActionInterval(int actionInterval)
        {
            _actionInterval = actionInterval;
        }

        public void ConfigureTwitch(string username, string accessToken, string channel)
        {
            var credentials = new ConnectionCredentials(username, accessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var webSocketClient = new WebSocketClient(clientOptions);

            _twitchClient = new TwitchClient(webSocketClient);
            _twitchClient.Initialize(credentials, channel);
            _twitchClient.OnJoinedChannel += OnJoinedChannel;
            _twitchClient.OnMessageReceived += OnMessageReceived;
        }

        public void SetLocale(string locale)
        {
            var localeFilePath = Path.Combine(".", "Locales", $"{locale}.json");
            _locale = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(localeFilePath));
            _votingManager.SetLocale(_locale);
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _channel = e.Channel;
            _twitchClient.SendMessage(e.Channel, string.Format(_locale["on_join_message"], _actionInterval / 60f));
            _runningBot = new Thread(async () =>
            {
                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    _twitchClient.SendMessage(_channel, _votingManager.StartNextVote());
                    await Task.Delay(_actionInterval * 1000);
                    _twitchClient.SendMessage(_channel, _votingManager.GetResult());
                }
                _twitchClient.Disconnect();
            });
            _runningBot.Start();
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            _votingManager.AddVote(e.ChatMessage.Message);
        }
    }
}