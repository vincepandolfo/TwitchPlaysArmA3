_logic = param [0, objNull, [objNull]];

_voteName = _logic getVariable ["Name", ""];
_entriesString = _logic getVariable ["Entries", ""];
_entries = parseSimpleArray _entriesString;
_numEntries = (count _entries) / 2;

_arguments = [_voteName, _numEntries];
_arguments append _entries;

"TwitchPlaysArmA3" callExtension ["add_vote", _arguments];

true;
