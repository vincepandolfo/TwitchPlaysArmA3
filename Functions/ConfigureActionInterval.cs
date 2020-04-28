using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class ConfigureActionInterval : IFunction
    {
        public override string GetName()
        {
            return "configure_action_interval";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            var actionInterval = int.Parse(args[0]);
            bot.SetActionInterval(actionInterval);
            output.Append($"Set action interval to {actionInterval}");
            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            if (args.Length != 1)
            {
                error = "1 parameter expected";
                return false;
            }

            if (!int.TryParse(args[0], out var _ignored))
            {
                error = $"Argument should be an integer, got {args[0]}";
                return false;
            }

            return true;
        }
    }
}