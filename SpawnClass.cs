using System.Collections.Generic;

namespace TwitchPlaysArmA3
{
    public class SpawnClass
    {
        public SpawnClass(string name, IEnumerable<string> areas, IEnumerable<string> types)
        {
            Name = name;
            Areas = areas;
            Types = types;
        }

        public string Name { get; private set; }
        public IEnumerable<string> Areas { get; private set; }
        public IEnumerable<string> Types { get; private set; }
    }
}