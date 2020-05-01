using System.Text;
using System.Linq;

namespace TwitchPlaysArmA3.Functions
{
    class AddVote : IFunction
    {
        public override string GetName()
        {
            return "add_vote";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            var name = Sanitize(args[0]);
            var optionCount = int.Parse(args[1]);
            var shortNames = args.Skip(2).Take(optionCount).Select(a => Sanitize(a));
            var fullNames = args.Skip(2 + optionCount).Take(optionCount).Select(t => Sanitize(t));

            bot.AddVote(name, shortNames, fullNames);

            output.Append($"Added vote {name} with {optionCount} possible options");
            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            if (args.Length < 4)
            {
                error = "At least 4 arguments expected";
                return false;
            }

            if (!int.TryParse(args[1], out var optionCount))
            {
                error = "Second argument must be an integer";
                return false;
            }

            if (args.Length != 2 + optionCount * 2)
            {
                error = $"Expected {2 + optionCount * 2}, got {args.Length}";
                return false;
            }

            return true;
        }
    }
}