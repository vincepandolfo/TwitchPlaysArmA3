using MoreLinq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TwitchPlaysArmA3.Voting
{

    class VotingManager
    {
        private readonly List<Vote> _votes = new List<Vote>();
        private Vote _currentVote = null;

        private IDictionary<string, int> _currentVotes = new ConcurrentDictionary<string, int>();

        public void AddVote(string name, IEnumerable<string> shortNames, IEnumerable<string> fullNames)
        {
            _votes.Add(new Vote(name, shortNames, fullNames));
        }

        public bool IsValidVote(string name)
        {
            return _votes.Any(v => v.Name == name);
        }

        public bool IsVoteHappening()
        {
            return _currentVote != null;
        }

        public string StartVote(string name)
        {
            _currentVote = _votes.Find(v => v.Name == name);

            foreach(var option in _currentVote.Options)
            {
                _currentVotes[option.FullName] = 0;
            }

            return _currentVote.DisplayableOptions();
        }

        public void AddVote(string vote)
        {
            var votedOption = _currentVote.GetMatching(vote);

            if (votedOption != null)
            {
                _currentVotes[votedOption.FullName]++;
            }
        }

        public string EndVote()
        {
            _currentVote = null;
            var result = _currentVotes.MaxBy(kv => kv.Value).Shuffle().First().Key;
            _currentVotes.Clear();
            return result;
        }
    }
}
