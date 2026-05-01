using UnityEngine;

public class startSceneFlappyBird : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }
}
