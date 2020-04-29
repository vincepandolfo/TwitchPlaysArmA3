using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TwitchPlaysArmA3.Functions
{
    class SetLocale : IFunction
    {
        public override string GetName()
        {
            return "set_locale";
        }

        protected override int Execute(StringBuilder output, string[] args)
        {
            var locale = Sanitize(args[0]);
            bot.SetLocale(locale);
            output.Append($"Set locale to {locale}");

            return 0;
        }

        protected override bool ValidateArguments(string[] args, out string error)
        {
            if (args.Length != 1)
            {
                error = "Expected 1 argument";
                return false;
            }

            var localesFolder = Path.Combine(@".\", "Locales");
            var validLocales = Directory.GetFiles(localesFolder, "*.json").Select(f => f.Replace(".json", "").Replace(@".\Locales\", ""));
            var locale = Sanitize(args[0]);
            if (!validLocales.Contains(locale))
            {
                error = $"{locale} is not a valid locale. Allowed: {string.Join(", ", validLocales)}";
                return false;
            }

            error = null;
            return true;
        }
    }
}