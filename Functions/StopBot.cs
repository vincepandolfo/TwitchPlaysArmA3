using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class StopBot : IFunction
    {
        public override string GetName()
        {
            return "stop";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            output.Append("Stopping the TwitchBot");
            bot.Stop();
            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            return true;
        }
    }
}