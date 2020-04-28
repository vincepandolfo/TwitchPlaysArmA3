using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class StartBot : IFunction
    {
        public override string GetName()
        {
            return "start";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            output.Append("Starting the TwitchBot");
            bot.Start();
            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            return true;
        }
    }
}