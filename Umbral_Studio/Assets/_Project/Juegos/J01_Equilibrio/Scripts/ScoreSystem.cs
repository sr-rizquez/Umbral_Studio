using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public float score = 0f;

    public TrayController tray;   
    public TMP_Text scoreText;    
    void Update()
    {
        if (tray == null) return;

        float angle = tray.GetCurrentAngle();

        // Zona de buen equilibrio
        if (Mathf.Abs(angle) < 10f)
        {
            score += Time.deltaTime * 10f;
        }

        // Mostrar puntos
        if (scoreText != null)
        {
            scoreText.text = Mathf.RoundToInt(score).ToString();
        }
    }
}