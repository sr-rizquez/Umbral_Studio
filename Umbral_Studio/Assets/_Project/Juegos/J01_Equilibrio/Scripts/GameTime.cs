using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    public float duration = 10f;

    private float timer;
    private bool finished = false;

    public TrayController tray;
    public TextMeshProUGUI timerText;

    void Start()
    {
        timer = duration;
    }

    void Update()
    {
        if (finished) return;

        timer -= Time.deltaTime;

        // 🔥 Mostrar tiempo en pantalla
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString();
        }

        if (timer <= 0)
        {
            finished = true;

            if (timerText != null)
                timerText.text = "0";

            Debug.Log("TIEMPO TERMINADO");

            if (tray != null)
                tray.ForceFall();
        }
    }
}