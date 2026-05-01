using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCooking : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape))
            GoWork();
    }
    
    public void Collect() { }
    
    public void GoWork() {
        SceneManager.LoadScene("SceneWorkPlace");
    }
}
