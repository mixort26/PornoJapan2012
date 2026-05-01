using UnityEngine;

public class startSceneWorkPlace : MonoBehaviour
{
    public AudioClip musicTheme;

    void Start() {
        MusicManager.Instance.PlayMusic(musicTheme);
    }
}
