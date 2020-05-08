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
            if (bot.Stop())
            {
                output.Append("Stopped the Bot successfully");
                return 0;
            }

            output.Append("Failed to stop the Bot");
            return -1;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            return true;
        }
    }
}