using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;

        text.text = "Preparado...";
        yield return new WaitForSecondsRealtime(1f);

        text.text = "Listos...";
        yield return new WaitForSecondsRealtime(1f);

        text.text = "YA!";
        yield return new WaitForSecondsRealtime(1f);

        text.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }
}