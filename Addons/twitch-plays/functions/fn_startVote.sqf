_logic = param [0, objNull, [objNull]];
_activated = param [2,false, [false]];

_voteName = _logic getVariable ["VoteName", -1];

if (_activated) then {
  "TwitchPlaysArmA3" callExtension ["start_vote", [_voteName]];
};

true;
