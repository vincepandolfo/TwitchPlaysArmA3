using MoreLinq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchPlaysArmA3.Voting
{
    enum VoteState
    {
        NOT_STARTED,
        VOTING_SPAWN_CLASS,
        VOTED_SPAWN_CLASS,
        VOTING_SPAWN_AREA,
        VOTED_SPAWN_AREA,
        VOTING_TYPE,
        VOTED_TYPE
    }

    class VotingManager
    {
        private readonly List<SpawnClass> _spawnClasses = new List<SpawnClass>();
        private SpawnClass _chosenSpawnClass;
        private string _chosenType;
        private IDictionary<string, string> _locale;
        private ExtensionCallback _callback;

        private IDictionary<string, int> _currentVotes = new ConcurrentDictionary<string, int>();
        private VoteState _currentState = VoteState.NOT_STARTED;
        public void AddSpawnClass(string name, IEnumerable<string> areas, IEnumerable<string> types)
        {
            _spawnClasses.Add(new SpawnClass(name, areas, types));
        }
        
        public void SetLocale(IDictionary<string, string> locale)
        {
            _locale = locale;
        }

        public void SetCallback(ExtensionCallback callback)
        {
            _callback = callback;
        }

        public VoteState GetState() { return _currentState; }

        public string StartNextVote()
        {
            _callback.Invoke("TwitchPlaysArmA3", "new_vote", _currentState.ToString());

            switch (_currentState)
            {
                case VoteState.NOT_STARTED:
                    _currentState = VoteState.VOTING_SPAWN_CLASS;
                    break;
                case VoteState.VOTED_SPAWN_CLASS:
                    _currentState = VoteState.VOTING_TYPE;
                    break;
                case VoteState.VOTED_TYPE:
                    _currentState = VoteState.VOTING_SPAWN_AREA;
                    break;
                case VoteState.VOTED_SPAWN_AREA:
                    _currentState = VoteState.VOTING_SPAWN_CLASS;
                    break;
                default:
                    throw new InvalidOperationException("Can't start a new vote during another");
            }
            _currentVotes.Clear();

            var builder = new StringBuilder();

            if (_currentState == VoteState.VOTING_SPAWN_CLASS)
            {
                builder.AppendLine(_locale["on_new_spawn_class_vote"]);

                _spawnClasses.ForEach(sc =>
                {
                    builder.AppendLine(sc.Name);
                });
            } else if (_currentState == VoteState.VOTING_SPAWN_AREA)
            {
                builder.AppendLine(_locale["on_new_area_vote"]);

                foreach (var a in _chosenSpawnClass.Areas)
                {
                    builder.AppendLine(a);
                }
            }
            else if (_currentState == VoteState.VOTING_TYPE)
            {
                builder.AppendLine(_locale["on_new_type_vote"]);

                foreach (var a in _chosenSpawnClass.Types)
                {
                    builder.AppendLine(a);
                }
            }

            return builder.ToString();
        }

        public void AddVote(string vote)
        {
            if (ValidVote(vote))
            {
                if (_currentVotes.ContainsKey(vote))
                {
                    _currentVotes[vote]++;
                } else
                {
                    _currentVotes[vote] = 1;
                }
            }
        }

        private bool ValidVote(string vote)
        {
            switch (_currentState)
            {
                case VoteState.VOTING_SPAWN_CLASS:
                    return _spawnClasses.Select(sc => sc.Name).Contains(vote);
                case VoteState.VOTING_SPAWN_AREA:
                    return _chosenSpawnClass.Areas.Contains(vote);
                case VoteState.VOTING_TYPE:
                    return _chosenSpawnClass.Types.Contains(vote);
                default:
                    return false;
            }
        }

        public string GetResult()
        {
            var winner = _currentVotes.MaxBy(kv => kv.Value).First().Key;

            _callback.Invoke("TwitchPlaysArmA3", "vote_result", winner);

            switch (_currentState)
            {
                case VoteState.VOTING_SPAWN_CLASS:
                    _chosenSpawnClass = _spawnClasses.Find(sc => sc.Name == winner);
                    _currentState = VoteState.VOTED_SPAWN_CLASS;
                    return string.Format(_locale["on_spawn_class_vote_complete"], winner);
                case VoteState.VOTING_TYPE:
                    _currentState = VoteState.VOTED_TYPE;
                    _chosenType = winner;
                    return string.Format(_locale["on_type_vote_complete"], winner);
                case VoteState.VOTING_SPAWN_AREA:
                    _currentState = VoteState.VOTED_SPAWN_AREA;
                    return string.Format(_locale["on_all_votes_complete"], _chosenSpawnClass.Name, _chosenType, winner);
                default:
                    throw new InvalidOperationException("Can't get a result on a closed vote");
            }
        }
    }
}
