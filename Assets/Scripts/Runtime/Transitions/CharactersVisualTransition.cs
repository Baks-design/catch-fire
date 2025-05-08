using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CatchFire
{
    public class CharactersVisualTransition : MonoBehaviour
    {
        [SerializeField] float fadeDuration = 0.3f;
        [SerializeField] float fadeDecay = 16f;
        [SerializeField] Image fadeOverlay;

        public Image FadeOverlay => fadeOverlay;

        void Start() => fadeOverlay.color = new Color(0f, 0f, 0f, 0f);

        public IEnumerator Fade(float targetAlpha)
        {
            var startAlpha = fadeOverlay.color.a;
            var elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                var newAlpha = Maths.ExpDecay(startAlpha, targetAlpha, fadeDecay, elapsed / fadeDuration);
                fadeOverlay.color = new Color(0f, 0f, 0f, newAlpha);
                yield return null;
            }
        }
    }
}