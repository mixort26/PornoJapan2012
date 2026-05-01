using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    private AudioSource _audioSource;

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GlobalMusicManager");
                _instance = go.AddComponent<MusicManager>();
                _instance._audioSource = go.AddComponent<AudioSource>();
                _instance._audioSource.loop = true;
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_audioSource.clip == clip) return;

        _audioSource.clip = clip;
        _audioSource.volume = 0.1f;
        _audioSource.Play();
    }
}