using UnityEngine;

public class startSceneSettings : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }
}
