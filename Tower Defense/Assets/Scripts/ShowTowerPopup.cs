using UnityEngine;
using UnityEditor;
using System.Collections;

public class ShowTowerPopup : EditorWindow {

	public static string towerName = "";
	public Transform trainable;

	[MenuItem("Example/ShowPopup Example")]
	public static void Init(GameObject tower) {
		ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup> ();
		window.position = new Rect (Screen.width / 2, Screen.height / 2, 150, 300);
		// trainable = ((GameObject)Instantiate (tower)).transform;


		window.ShowPopup ();

		// pause game
		Time.timeScale = 0;

		// initTrain();
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
			
		// Instanatiate background
		//GUI.backgroundColor = Color.red;

		// Instantiate tower

		// Randomly spawn 10 enemies and allies
		// Texture2d trainable = AssetDatabase.LoadAssetAtPath("Materials/Enemy1Big.png", typeof());
		// (Texture2D) enemy = EditorGUILayout.ObjectField ("enemy", enemy, typeof(Texture2D), false);
	}

}
