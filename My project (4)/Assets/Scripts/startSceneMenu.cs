using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneMenu : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }

    public void GoSettings() {
        SceneManager.LoadScene("SceneSettings");
    }

    public void GoWork() {
        SceneManager.LoadScene("SceneWorkPlace");
    }

    public void GoQuit() {
        Application.Quit();
    }
}
