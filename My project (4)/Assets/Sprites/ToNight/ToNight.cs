using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sprites.ToNight
{
    public class SpritesToNight : MonoBehaviour
    {
        public AudioClip musicTheme;
        public Image[] images;
        private float _duration = 3f;

        private void Start() {
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("SceneFlappyBird");
                SceneManager.UnloadSceneAsync("ToNight");
            }
            MusicManager.Instance.PlayMusic(musicTheme);
            StartCoroutine(StartCor());
            StartCoroutine(End());
        }

        private IEnumerator End() {
            yield return new WaitForSeconds(_duration * images.Length + 5f);
            SceneManager.LoadScene("SceneFlappyBird");
            SceneManager.UnloadSceneAsync("ToNight");
        }

        private IEnumerator StartCor() {
            foreach (var i in images) {
                yield return new WaitForSeconds(3f);
                Appear(i, 1);
            }
        }
        
        public void Appear(Image sprite, float duration)
        {
            if (images == null) return;
            StartCoroutine(FadeRoutine(sprite, duration));
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