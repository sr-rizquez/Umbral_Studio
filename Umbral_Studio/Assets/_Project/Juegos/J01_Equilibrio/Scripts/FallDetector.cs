using UnityEngine;
using TMPro;
using System.Collections;

public class FallDetector : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public ScoreSystem scoreSystem;

    [Header("Audio")]
    public AudioSource gameAudio;
    public AudioSource gameOverSound;

    [Header("Fade")]
    public float fadeDuration = 1.5f;

    private bool gameEnded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Objeto"))
        {
            gameEnded = true;

            //  FADE OUT música
            if (gameAudio != null)
            {
                StartCoroutine(FadeOutAudio());
            }

            //  MOSTRAR GAME OVER
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            //  SCORE FINAL
            if (finalScoreText != null && scoreSystem != null)
            {
                int finalScore = Mathf.RoundToInt(scoreSystem.score);
                finalScoreText.text = "<size=35>SCORE:</size> <size=60>" + finalScore + "</size>";
            }

            // 🔥 SONIDO GAME OVER
            if (gameOverSound != null)
            {
                gameOverSound.Play();
            }

            // PAUSA 
            StartCoroutine(PauseAfterDelay(0.3f));
        }
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = gameAudio.volume;

        while (gameAudio.volume > 0)
        {
            gameAudio.volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        gameAudio.Stop();
        gameAudio.volume = startVolume;
    }

    IEnumerator PauseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }
}