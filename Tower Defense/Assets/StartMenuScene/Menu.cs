using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Canvas quitPopUp;
    public Canvas startHolder;
    public Button start;
    public Button quit;

	// Use this for initialization
	void Start () {
        quitPopUp = quitPopUp.GetComponent<Canvas>();
        startHolder = startHolder.GetComponent<Canvas>();
        start = start.GetComponent<Button>();
        quit = quit.GetComponent<Button>();
        quitPopUp.enabled = false;
	}
	
	public void QuitPress() { 
        startHolder.enabled = false;
        quitPopUp.enabled = true;
    }

    public void noPress() {
        quitPopUp.enabled = false;
        startHolder.enabled = true;
    }

    public void startGame() {
        SceneManager.LoadScene("TowerDefenseScene");
		SceneManager.UnloadScene ("Start Menu");
    }

    public void quitGame() {
        Application.Quit();
    }
}
