using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneSettings : MonoBehaviour
{
    public void GoExit() {
        SceneManager.LoadScene("SceneMenu");
    }
}