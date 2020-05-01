using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchPlaysArmA3
{
    public class Vote
    {
        public Vote(string name, IEnumerable<string> shortNames, IEnumerable<string> fullNames)
        {
            Name = name;
            Options = shortNames.Zip(fullNames, (sn, ln) => new Option { ShortName = sn, FullName = ln });
        }

        public string Name { get; private set; }
        public IEnumerable<Option> Options { get; private set; }

        public string DisplayableOptions()
        {
            var builder = new StringBuilder();

            foreach (var option in Options)
            {
                builder.AppendLine($"{option.ShortName} - {option.FullName}");
            }

            return builder.ToString();
        }

        public Option GetMatching(string vote)
        {
            return Options.FirstOrDefault(o => o.Matches(vote));
        }
    }

    public class Option
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }

        public bool Matches(string vote)
        {
            return ShortName.ToLower() == vote.ToLower() || FullName.ToLower() == vote.ToLower();
        }
    }
}