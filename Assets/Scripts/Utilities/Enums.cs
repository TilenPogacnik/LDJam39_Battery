public static class Enums{
	public static class Tags {
		public const string Player = "Player";
		public const string Block = "Block";
		public const string PlayerDeath = "PlayerDeath";
		public const string KillZone = "KillZone";
	}

	public enum GameState {
		Playing,
		GameOver,
		MainMenu
	}
}