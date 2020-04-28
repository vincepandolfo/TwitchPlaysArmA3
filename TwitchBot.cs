using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchPlaysArmA3
{
    public class TwitchBot
    {
        private TwitchClient _twitchClient;
        private int _actionInterval = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
        private ExtensionCallback _callback;
        private readonly List<SpawnClass> _spawnClasses = new List<SpawnClass>();

        private CancellationTokenSource _tokenSource;
        private Task _runningBot;

        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            _runningBot = Task.Factory.StartNew(async () =>
              {
                  _twitchClient.Connect();
                  while (!_tokenSource.Token.IsCancellationRequested)
                  {

                      _callback("twitch_plays_arma3", "hint", "Test");
                      await Task.Delay(_actionInterval);
                  }
                  _twitchClient.Disconnect();
              }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _runningBot.Wait();
        }

        public void AddSpawnClass(string name, IEnumerable<string> areas, IEnumerable<string> types)
        {
            _spawnClasses.Add(new SpawnClass(name, areas, types));
        }

        public void SetCallback(ExtensionCallback callback)
        {
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
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _twitchClient.SendMessage(e.Channel, "Hello! This is the ArmA3 bot");
        }
    }
}