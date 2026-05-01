using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneSettings : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }

    public void GoExit() {
        SceneManager.LoadScene("SceneMenu");
    }
}
