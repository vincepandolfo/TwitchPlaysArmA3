using System;
using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class ConfigureTwitch : IFunction
    {
        public override string GetName()
        {
            return "configure_twitch";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            var appName = Sanitize(args[0]);
            var clientId = Sanitize(args[1]);
            var clientSecret = Sanitize(args[2]);
            var accessToken = Sanitize(args[3]);
            var refreshToken = Sanitize(args[4]);
            var channel = Sanitize(args[5]);

            bot.ConfigureTwitch(appName, clientId, clientSecret, accessToken, refreshToken, channel);
            output.Append($"Twitch access configured for user {appName} in channel {channel}");

            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            if (args.Length != 6)
            {
                error = "6 parameters expected";
                return false;
            }

            error = null;
            return true;
        }
    }
}