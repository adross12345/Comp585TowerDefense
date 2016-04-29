using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public void startNewGame() {
        SceneManager.LoadScene("TowerDefenseScene");
    }
}
