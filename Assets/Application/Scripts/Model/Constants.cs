namespace Application.Scripts.Model
{
	public class Constants
	{
		public class Scenes
		{
			public const string MAIN_MENU = "MainMenu";
			public const string GAMEPLAY = "Gameplay";
			public const string SCORE = "Score";
		}
	
		public class Layers
		{
			public const int OBSTACLE = 9;
			public const int SIMPLE_COIN = 11;
		}
	
		public class Tags
		{
			public const string CHARACTER = "Personaje";
			public const string DOOR_TRIGGER = "DoorTrigger";
		}
	
		public class AnimationParams
		{
			public const string DIE = "Die";
			public const string DASH_LEFT = "DashLeft";
			public const string DASH_RIGHT = "DashRight";
			public const string ROLL_UP = "RollUp";
			public const string DOWN_FALL = "DownFall";
			public const string SPEED_UP_START = "SpeedUpStart";
			public const string SPEED_UP_EXIT = "SpeedUpExit";
			public const string OPEN_DOOR = "OpenDoor";
		}
	
		public class PooledObjects
		{
			public const string RED_PLATFORM = "RedPlatform";
			public const string ELEVATOR = "Elevator";
			public const string SECURITY_BOT = "SecurityBot";
			public const string OBSTACLE_COINS = "ObstacleCoins";
			public const string COINS = "Coins";
			public const string POWERUP_SHIELD = "Shield";
			public const string POWERUP_COINIMAN = "CoinIman";
			public const string POWERUP_SPEEDUP = "SpeedUp";
			public const string BG_FOREST = "BGForest";
			public const string BG_CAVE_ENTRACE = "BGCaveEntrace";
			public const string BG_CAVE_EXIT = "BGCaveExit";
			public const string BG_CAVE = "BGCave";
			public const string BG_CASCADE = "BGCascade";
			public const string BG_CASCADE_ENTRACE = "BGCascadeEntrace";
			public const string BG_CASCADE_EXIT = "BGCascadeExit";
		}
	
		public class Audio
		{
			public const string MENU_THEME = "MenuTheme";
			public const string GAME_THEME = "GameTheme";
			public const string COIN = "Coin";
		}

		public class Language
		{
			public const string LANGUAGE = "Language";
			public const string LANG_ESP_KEY = "ESP";
			public const string LANG_ENG_KEY = "ENG";
		}
	}
}