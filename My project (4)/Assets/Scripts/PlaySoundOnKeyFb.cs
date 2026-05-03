using UnityEngine;

public class PlaySoundOnKeyFb : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Получаем ссылку на компонент Audio Source
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Проверяем нажатие клавиши (например, Пробел)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // PlayOneShot позволяет звукам накладываться друг на друга
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}