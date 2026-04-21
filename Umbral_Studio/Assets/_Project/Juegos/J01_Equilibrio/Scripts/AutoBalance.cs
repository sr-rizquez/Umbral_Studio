using UnityEngine;

public class AutoBalance : MonoBehaviour
{
    [Header("Ángulo automático")]
    public float maxAngle = 30f;  // ← lo del documento
    public float speed = 1f;

    private float autoAngle;

    void Update()
    {
        // Oscila entre -maxAngle y +maxAngle
        autoAngle = Mathf.Sin(Time.time * speed) * maxAngle;
    }

    public float GetAutoAngle()
    {
        return autoAngle;
    }
}