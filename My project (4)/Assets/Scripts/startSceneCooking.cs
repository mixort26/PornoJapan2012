using UnityEngine;

public class startSceneCooking : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }
}
