using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

    public Canvas quitScreen;
    private static float previousTimeScale = 1f;

    void Start() {
        quitScreen.enabled = false;
    }

    public void pressQuit() {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
        quitScreen.enabled = true;
    }

    public void quit() {
        Application.Quit();
    }

    public void resume() {
        quitScreen.enabled = false;
        Time.timeScale = previousTimeScale;
    }
}
