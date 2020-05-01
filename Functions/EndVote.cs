using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class EndVote : IFunction
    {
        public override string GetName()
        {
            return "end_vote";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            if (bot.EndVote(out var result))
            {
                output.Append(result);
                return 0;
            }

            output.Append("N/A");
            return -1;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            return true;
        }
    }
}