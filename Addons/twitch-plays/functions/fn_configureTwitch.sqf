_logic = param [0, objNull, [objNull]];

_appName = _logic getVariable ["AppName", -1];
_channel = _logic getVariable ["Channel", -1];
_clientId = _logic getVariable ["ClientId", -1];
_clientSecret = _logic getVariable ["ClientSecret", -1];
_accessToken = _logic getVariable ["AccessToken", -1];
_refreshToken = _logic getVariable ["RefreshToken", -1];
_locale = _logic getVariable ["Locale", -1];

"TwitchPlaysArmA3" callExtension ["configure_twitch", [_appName, _clientId, _clientSecret, _accessToken, _refreshToken, _channel]];
"TwitchPlaysArmA3" callExtension ["set_locale", [_locale]];
"TwitchPlaysArmA3" callExtension "start";

true;
