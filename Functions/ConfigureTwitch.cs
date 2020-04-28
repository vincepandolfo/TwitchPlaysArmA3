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
            var username = Sanitize(args[0]);
            var accessToken = Sanitize(args[1]);
            var channel = Sanitize(args[2]);

            bot.ConfigureTwitch(username, accessToken, channel);
            output.Append($"Twitch access configured for user {username} in channel {channel}");

            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            if (args.Length != 3)
            {
                error = "3 parameters expected";
                return false;
            }

            error = null;
            return true;
        }
    }
}