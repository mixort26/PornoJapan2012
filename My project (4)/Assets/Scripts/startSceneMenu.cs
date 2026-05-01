using UnityEngine;

public class startSceneMenu : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }
}
