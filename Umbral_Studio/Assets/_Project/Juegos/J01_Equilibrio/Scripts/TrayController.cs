using UnityEngine;

public class TrayController : MonoBehaviour
{
    [Header("Configuración jugador")]
    public float sensitivity = 200f;   // Fuerza del control
    public float maxAngle = 30f;       // Límite total
    public float smooth = 5f;          // Suavizado
    public float deadZone = 0.02f;     // Zona muerta

    [Header("Referencia")]
    public AutoBalance autoBalance;    // Fuerza automática

    private float baseInput;
    private float currentAngle;

    void Start()
    {
        baseInput = Input.acceleration.x;
    }

    void Update()
    {
        float raw = Input.acceleration.x;

        //  Anti drift (recalibración)
        baseInput = Mathf.Lerp(baseInput, raw, Time.deltaTime * 0.5f);

        float input = raw - baseInput;

        //  Zona muerta
        if (Mathf.Abs(input) < deadZone)
            input = 0f;

        //  Ángulo del jugador
        float playerAngle = input * sensitivity;

        //  Ángulo automático
        float autoAngle = 0f;
        if (autoBalance != null)
            autoAngle = autoBalance.GetAutoAngle();

        //  Combinación final
        float targetAngle = playerAngle + autoAngle;

        //  Limitar inclinación
        targetAngle = Mathf.Clamp(targetAngle, -maxAngle, maxAngle);

        //  Suavizado
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * smooth);

        //  Aplicar rotación
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    // PARA EL SISTEMA DE PUNTOS
    public float GetCurrentAngle()
    {
        return currentAngle;
    }

    // PARA LA CAÍDA FINAL FORZADA
    public void ForceFall()
    {
        currentAngle = maxAngle;
    }
}