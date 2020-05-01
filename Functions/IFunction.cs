using System.Text;
using TwitchPlaysArmA3.Bot;

namespace TwitchPlaysArmA3.Functions
{
    abstract class IFunction
    {
        protected TwitchBot bot;

        public abstract string GetName();
        public int Call(StringBuilder output, string[] args)
        {
            if (ValidateArguments(args, out var error))
            {
                return Execute(output, args);
            }

            output.Append($"Arguments provided for function {GetName()} invalid: {error}");
            return -1;
        }

        public void SetBot(TwitchBot bot)
        {
            this.bot = bot;
        }

        protected string Sanitize(string s)
        {
            return s.Replace("\"", "");
        }

        protected abstract int Execute(StringBuilder output, string[] args);

        protected abstract bool ValidateArguments(string[] args, out string error);
    }
}