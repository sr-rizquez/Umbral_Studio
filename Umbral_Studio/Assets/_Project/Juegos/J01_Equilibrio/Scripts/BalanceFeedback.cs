using UnityEngine;
using UnityEngine.UI;

public class BalanceFeedback : MonoBehaviour
{
    [Header("Referencias")]
    public Image balanceBar;
    public TrayController tray;

    [Header("Configuración")]
    public float greenZone = 10f;
    public float yellowZone = 20f;
    public float smoothSpeed = 5f;

    [Header("Vibración")]
    public float vibrationInterval = 0.3f;

    private Color targetColor;
    private float vibrationTimer = 0f;

    void Update()
    {
        if (tray == null || balanceBar == null) return;

        float angle = Mathf.Abs(tray.GetCurrentAngle());

        //  COLOR SEGÚN EQUILIBRIO
        if (angle < greenZone)
        {
            targetColor = Color.green;
            vibrationTimer = 0f;
        }
        else if (angle < yellowZone)
        {
            targetColor = Color.yellow;
            vibrationTimer = 0f;
        }
        else
        {
            targetColor = Color.red;

            //  VIBRACIÓN CONTINUA
            vibrationTimer += Time.deltaTime;

            if (vibrationTimer >= vibrationInterval)
            {
                Vibrate();
                vibrationTimer = 0f;
            }
        }

        //  TRANSICIÓN SUAVE
        balanceBar.color = Color.Lerp(balanceBar.color, targetColor, Time.deltaTime * smoothSpeed);
    }

    //  VIBRACIÓN COMPATIBLE CON ANDROID
    void Vibrate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                if (vibrator != null)
                {
                    vibrator.Call("vibrate", 200); // duración en ms
                }
            }
        }
        catch
        {
            Handheld.Vibrate();
        }
#else
        Handheld.Vibrate();
#endif
    }
}