class CfgPatches
{
	class TwitchPlays
	{
		units[] = { "ConfigureTwitchModule", "CreateVoteModule" };
		requiredVersion = 1.0;
		requiredAddons[] = {"A3_Modules_F"};
	};
};

class CfgFactionClasses
{
	class NO_CATEGORY;

	class Twitch: NO_CATEGORY
	{
		displayName = "TwitchPlaysArmA3";
	};
};

class CfgFunctions 
{
	class TwitchPlays
	{
		class Twitch
		{
			file = "twitch-plays\functions";
			class configureTwitch{};
			class createVote{};
			class startVote{};
			class endVote{};
		};
	};
};

class CfgVehicles
{
	class Logic;
	class Module_F: Logic
	{
		class AttributesBase
		{
			class Edit;
			class EditCodeMulti3 {
				control = "EditCodeMulti3";
			};
			class ModuleDescription;
		};

		class ModuleDescription
		{
		};
	}

	class ConfigureTwitchModule: Module_F
	{
		scope = 2;
		displayName = "Configure Twitch";
		category = "Twitch";

		isGlobal = 0;
		isTriggerActivated = 0;
		isDisposable = 1;
		is3DEN = 0;

		function = "TwitchPlays_fnc_configureTwitch";

		curatorInfoType = "RscDisplayAttributeConfigureTwitchModule";

		class Attributes: AttributesBase
		{
			class AppName: Edit
			{
				displayName = "Appplication Name";
				tooltip = "Name of the Twitch App used to generate credentials";
				defaultValue = """ChatPlaysArmA3""";
				property = "AppName";
			};

			class Channel: Edit
			{
				displayName = "Channel";
				tooltip = "Channel on which votes will be posted";
				defaultValue = """specialassaultsquad2""";
				property = "Channel";
			};

			class ClientId: Edit
			{
				displayName = "Client Id";
				tooltip = "Client Id of the App used to generate credentials";
				property = "ClientId";
			};

			class ClientSecret: Edit
			{
				displayName = "Client Secret";
				tooltip = "Client Secret of the App used to generate credentials";
				property = "ClientSecret";
			};

			class AccessToken: Edit
			{
				displayName = "Access Token";
				tooltip = "Twitch access token";
				property = "AccessToken";
			};

			class RefreshToken: Edit
			{
				displayName = "Refresh Token";
				tooltip = "Twitch refresh token";
				property = "RefreshToken";
			};

			class Locale: Edit
			{
				displayName = "Language";
				tooltip = "Language to use for messages to Twitch";
				property = "Locale";
				defaultValue = """IT""";
			};

			class ModuleDescription: ModuleDescription {};
		}

		class ModuleDescription: ModuleDescription
		{
			description = "Configure the Twitch extension";
			sync[] = {};
		};
	}

	class CreateVoteModule: Module_F
	{
		scope = 2;
		displayName = "Create vote";
		category = "Twitch";

		isGlobal = 0;
		isTriggerActivated = 0;
		isDisposable = 1;
		is3DEN = 0;

		curatorInfoType = "RscDisplayAttributeCreateVoteModule";

		function = "TwitchPlays_fnc_createVote";

		class Attributes: AttributesBase
		{
			class Name: Edit
			{
				displayName = "Name";
				tooltip = "Name of the vote";
				defaultValue = """My Vote""";
				property = "Name";
			};

			class Entries: Edit
			{
				displayName = "Entries";
				tooltip = "Entries for the vote as an array. Short names first, then full";
				defaultValue = """['R', 'G', 'B', 'Red', 'Green', 'Blue']""";
				property = "Entries";
			};

			class ModuleDescription: ModuleDescription {};
		}

		class ModuleDescription: ModuleDescription
		{
			description = "Creates a vote that can then be run on Twitch";
			sync[] = {};
		};
	}

	class StartVoteModule: Module_F
	{
		scope = 2;
		displayName = "Start vote";
		category = "Twitch";

		isGlobal = 0;
		isTriggerActivated = 1;
		isDisposable = 1;
		is3DEN = 0;

		curatorInfoType = "RscDisplayAttributeStartVoteModule";

		function = "TwitchPlays_fnc_startVote";

		class Attributes: AttributesBase
		{
			class VoteName: Edit
			{
				displayName = "Vote name";
				tooltip = "Name of the vote";
				defaultValue = """My Vote""";
				property = "VoteName";
			};

			class ModuleDescription: ModuleDescription {};
		}

		class ModuleDescription: ModuleDescription
		{
			description = "Starts a vote on Twitch";
			sync[] = {};
		};
	}

	class EndVoteModule: Module_F
	{
		scope = 2;
		displayName = "Stop vote";
		category = "Twitch";

		isGlobal = 0;
		isTriggerActivated = 1;
		isDisposable = 1;
		is3DEN = 0;

		curatorInfoType = "RscDisplayAttributeStopVoteModule";

		function = "TwitchPlays_fnc_endVote";

		class Attributes: AttributesBase
		{
			class PostVoteScript: EditCodeMulti3
			{
				displayName = "Post vote script";
				tooltip = "Code to execute when the vote is ended. You can access the result via `_this`";
				defaultValue = """hint _this;""";
				property = "PostVoteScript";
				expression = "_this setVariable ['TwitchPlays_PostVoteScript', _value, true]";
			};

			class ModuleDescription: ModuleDescription {};
		}

		class ModuleDescription: ModuleDescription
		{
			description = "Starts a vote on Twitch";
			sync[] = {};
		};
	}
}
