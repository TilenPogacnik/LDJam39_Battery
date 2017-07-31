using UnityEngine;

public static class Events {
	private static bool debugMessages = true;

	public delegate void GameAction();
	public static event GameAction gameStarted;
	public static event GameAction gameRestarted;
	public static event GameAction gameEnded;
	public static event GameAction playerDied;

	public static void OnGameStarted(){
		if (debugMessages) {
			Debug.Log ("Events triggered: " + System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		if (gameStarted != null) {
			gameStarted ();
		}
	}

	public static void OnGameEnded(){
		if (debugMessages) {
			Debug.Log ("Events triggered: " + System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		if (gameEnded != null) {
			gameEnded ();
		}
	}

	public static void OnPlayerDied(){
		if (debugMessages) {
			Debug.Log ("Events triggered: " + System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		if (playerDied != null) {
			playerDied ();
		}
	}

	public static void OnGameRestarted(){
		if (debugMessages) {
			Debug.Log ("Events triggered: " + System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		if (gameRestarted != null) {
			gameRestarted ();
		}
	}
}