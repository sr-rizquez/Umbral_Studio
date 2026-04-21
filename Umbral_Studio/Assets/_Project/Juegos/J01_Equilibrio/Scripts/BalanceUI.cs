using UnityEngine;

public class BalanceUI : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform indicator;   // el marcador
    public TrayController tray;       // la bandeja

    [Header("Configuración")]
    public float maxMove = 250f;      // límite visual (ajusta según tu barra)

    void Update()
    {
        if (tray == null || indicator == null)
            return;

        // Obtener ángulo actual
        float angle = tray.GetCurrentAngle();

        // Normalizar (-1 a 1)
        float normalized = angle / tray.maxAngle;

        // LIMITAR para que no salga de la barra
        normalized = Mathf.Clamp(normalized, -1f, 1f);

        // Convertir a posición
        float posX = normalized * maxMove;

        // Aplicar movimiento
        indicator.anchoredPosition = new Vector2(posX, 0);
    }
}