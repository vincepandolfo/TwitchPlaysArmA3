using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class StartVote : IFunction
    {
        public override string GetName()
        {
            return "start_vote";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            if (bot.StartVote(Sanitize(args[0]))) {
                output.Append($"Started vote for {args[0]}");

                return 0;
            }

            output.Append($"{args[0]} is not a known vote");
            return -1;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            if (args.Length != 1)
            {
                error = "Expected exactly 1 argument";
                return false;
            }

            error = null;
            return true;
        }
    }
}