using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchPlaysArmA3.Voting;

namespace TwitchPlaysArmA3.Bot
{
    public class TwitchBot
    {
        private TwitchClient _twitchClient;
        private IDictionary<string, string> _locale;
        private VotingManager _votingManager = new VotingManager();
        private string _channel;
        private Thread _runningBot;
        private CancellationTokenSource _tokenSource;

        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            _runningBot = new Thread(() =>
            {
                _twitchClient.Connect();

                _tokenSource.Token.WaitHandle.WaitOne();

                _twitchClient.Disconnect();
            });
            _runningBot.Start();
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _runningBot.Join();
        }

        public void AddVote(string name, IEnumerable<string> shortNames, IEnumerable<string> fullNames)
        {
            _votingManager.AddVote(name, shortNames, fullNames);
        }

        public bool StartVote(string name)
        {
            if (!_votingManager.IsValidVote(name))
            {
                return false;
            }

            var options = _votingManager.StartVote(name);

            _twitchClient.SendMessage(_channel, string.Format(_locale["on_new_vote"], name));

            foreach (var option in options.Split(new[] { Environment.NewLine },StringSplitOptions.None))
            {
                _twitchClient.SendMessage(_channel, option);
            }

            return true;
        }

        public bool EndVote(out string result)
        {
            if (!_votingManager.IsVoteHappening())
            {
                result = null;
                return false;
            }

            result = _votingManager.EndVote();

            _twitchClient.SendMessage(_channel, string.Format(_locale["on_vote_completed"], result));

            return true;
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
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _channel = e.Channel;
            _twitchClient.SendMessage(e.Channel, string.Format(_locale["on_join_message"]));
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            _votingManager.AddVote(e.ChatMessage.Message);
        }
    }
}