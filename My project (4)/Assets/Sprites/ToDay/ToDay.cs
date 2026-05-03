using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sprites.ToDay
{
    public class SpritesToDay : MonoBehaviour
    {
        public AudioClip musicTheme;
        public Image[] images;
        private float _duration = 4f;

        private void Start() {
            MusicManager.Instance.PlayMusic(musicTheme);
            StartCoroutine(StartCor());
            StartCoroutine(End());
        }
        
        private void Update() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                SceneManager.LoadScene("SceneWorkPlace", LoadSceneMode.Additive);
            }
        }
        
        private IEnumerator End() {
            yield return new WaitForSeconds(_duration * images.Length + 5f);
            SceneManager.LoadScene("SceneWorkPlace",  LoadSceneMode.Additive);
        }

        private IEnumerator StartCor() {
                yield return new WaitForSeconds(_duration/2);
            foreach (var i in images) {
                Appear(i, 1);
                yield return new WaitForSeconds(_duration);
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