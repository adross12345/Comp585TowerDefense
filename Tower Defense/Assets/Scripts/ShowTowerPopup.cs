using UnityEngine;
using UnityEditor;
using System.Collections;

public class ShowTowerPopup : EditorWindow {

	public static string towerName = "";

	[MenuItem("Example/ShowPopup Example")]
	public static void Init(GameObject tower) {
		ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 150, 300);
		window.ShowPopup();

		// pause game
		towerName = tower.ToString();
		Time.timeScale = 0;
		// initate training
	}

	private void initTrain(GameObject tower) {

	}

	void OnGUI() {
		
		EditorGUILayout.LabelField("Tower Training",EditorStyles.wordWrappedLabel);
		GUILayout.Space(70);
		if (GUILayout.Button ("Done!")) {
			this.Close ();
			Time.timeScale = 1;
		}
	}

}
