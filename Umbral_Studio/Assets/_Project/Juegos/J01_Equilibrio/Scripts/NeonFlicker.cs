using UnityEngine;
using TMPro;

public class NeonFlicker : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float speed = 6f;
    public float minAlpha = 0.4f;
    public float maxAlpha = 1f;

    private float timer;

    void Update()
    {
        if (text == null) return;

        timer += Time.unscaledDeltaTime * speed;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(timer) + 1f) / 2f);

        Color color = new Color(0.22f, 1f, 0.08f, alpha); // verde neon
        text.color = color;
    }
}