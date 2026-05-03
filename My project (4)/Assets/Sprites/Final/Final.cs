using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sprites.Final
{
    public class SpritesFinal : MonoBehaviour
    {
        public AudioClip musicTheme;
        public Image[] images;
        public Image blackscreen;
        private float _duration = 6f;

        private void Start() {
            MusicManager.Instance.PlayMusic(musicTheme);
            StartCoroutine(StartCor());
            StartCoroutine(End());
        }

        private IEnumerator End() {
            yield return new WaitForSeconds(_duration * images.Length + 15f);
            SceneManager.LoadScene("SceneMenu");
            SceneManager.UnloadSceneAsync("Final");
        }
        
        private IEnumerator StartCor() {
            StartCoroutine(FadeRoutineReverse(blackscreen, 5f));
            foreach (var i in images) {
                yield return new WaitForSeconds(_duration);
                Appear(i, 1.5f);
            }
        }
        
        public void Appear(Image sprite, float duration)
        {
            if (images == null) return;
            StartCoroutine(FadeRoutine(sprite, duration));
        }
        private IEnumerator FadeRoutineReverse(Image sprite, float duration)
        {
            float elapsed = 0;
            Color color = sprite.color;

            color.a = 1f;
            sprite.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
        
                color.a = Mathf.Clamp01(1f - (elapsed / duration));
                sprite.color = color;
        
                yield return null;
            }

            color.a = 0f;
            sprite.color = color;
        }
        private IEnumerator FadeRoutine(Image sprite, float duration)
        {
            float elapsed = 0;
            Color color = sprite.color;

            color.a = 0;
            sprite.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
            
                color.a = Mathf.Clamp01(elapsed / duration);
                sprite.color = color;
            
                yield return null;
            }

            color.a = 1f;
            sprite.color = color;
        }
    }
}