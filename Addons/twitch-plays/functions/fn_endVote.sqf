_logic = param [0, objNull, [objNull]];
_activated = param [2, false];

if (_activated) then {
  _result = "TwitchPlaysArmA3" callExtension "end_vote";

  _postVoteScript = _logic getVariable ["TwitchPlays_PostVoteScript", ""];
  _result call compile _postVoteScript;
};

true;
