using System.Text;
using System.Linq;

namespace TwitchPlaysArmA3.Functions
{
    class AddSpawnClass : IFunction
    {
        public override string GetName()
        {
            return "add_spawn_class";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            var name = Sanitize(args[0]);
            var areaCount = int.Parse(args[1]);
            var typeCount = int.Parse(args[2]);
            var areas = args.Skip(3).Take(areaCount).Select(a => Sanitize(a));
            var types = args.Skip(3 + areaCount).Take(typeCount).Select(t => Sanitize(t));

            bot.AddSpawnClass(name, areas, types);

            output.Append($"Added spawn class {name} with {areaCount} possible spawn areas and of {typeCount} different types");
            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            error = null;
            if (args.Length < 3)
            {
                error = "At least 3 arguments expected";
                return false;
            }

            if (!int.TryParse(args[1], out var areaCount) || !int.TryParse(args[2], out var typeCount))
            {
                error = "Second and third argument must be integers";
                return false;
            }

            if (args.Length != 3 + areaCount + typeCount)
            {
                error = $"Expected {3 + areaCount + typeCount}, got ${args.Length}";
                return false;
            }

            return true;
        }
    }
}