using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManagerLavado : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI platosTotalesText;

    [Header("Panel Final")]
    public GameObject panelFinal;
    public TextMeshProUGUI puntosFinalesTexto;

    [Header("Ajustes del Juego")]
    public GameObject[] manchasPrefabs; // Lista de tus 3 manchas
    public RectTransform zonaManchas;
    public float tiempoInicial = 15f;
    public float distanciaMinima = 100f;
    public int puntosPorMancha = 1; // Puntos ajustados a 1

    [Header("Audio")]
    public AudioSource musicaJuego;

    private float tiempoRestante;
    private int puntuacion = 0;
    private int platosContador = 0;
    private int manchasRestantesEnEstePlato;
    private bool juegoActivo = false;

    private List<Vector2> posicionesUsadas = new List<Vector2>();

    void Start()
    {
        if (musicaJuego == null) musicaJuego = GameObject.Find("MusicaTecno")?.GetComponent<AudioSource>();
        if (panelFinal != null) panelFinal.SetActive(false);

        tiempoRestante = tiempoInicial;
        puntuacion = 0;
        platosContador = 0;

        ActualizarInterfaz();
        juegoActivo = true;

        if (musicaJuego != null)
        {
            musicaJuego.volume = 0.4f;
            musicaJuego.Play();
        }

        GenerarNuevasManchas();
    }

    void Update()
    {
        if (!juegoActivo) return;

        // Cronómetro
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            timerText.text = "TIEMPO: " + Mathf.Ceil(tiempoRestante).ToString();
        }
        else
        {
            FinalizarJuego();
        }

        // --- EL SISTEMA QUE FUNCIONABA ---
        // Detecta toque en cualquier parte y borra la mancha que toque
        bool click = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                     (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame) ||
                     (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (click)
        {
            EliminarUnaMancha();
        }
    }

    void GenerarNuevasManchas()
    {
        foreach (Transform hijo in zonaManchas) { Destroy(hijo.gameObject); }
        posicionesUsadas.Clear();

        if (manchasPrefabs == null || manchasPrefabs.Length == 0) return;

        manchasRestantesEnEstePlato = Random.Range(3, 6);

        for (int i = 0; i < manchasRestantesEnEstePlato; i++)
        {
            float mx = zonaManchas.rect.width / 2.5f;
            float my = zonaManchas.rect.height / 2.5f;
            Vector2 posicionFinal = new Vector2(Random.Range(-mx, mx), Random.Range(-my, my));

            posicionesUsadas.Add(posicionFinal);

            // Instanciar una mancha aleatoria de la lista
            GameObject prefabAleatorio = manchasPrefabs[Random.Range(0, manchasPrefabs.Length)];
            GameObject m = Instantiate(prefabAleatorio, zonaManchas);
            m.GetComponent<RectTransform>().anchoredPosition = posicionFinal;
        }
    }

    // Esta función borra la primera mancha que encuentra en la zona, sin complicaciones
    void EliminarUnaMancha()
    {
        if (!juegoActivo || manchasRestantesEnEstePlato <= 0) return;

        if (zonaManchas.childCount > 0)
        {
            // Borra el primer hijo (la mancha)
            Destroy(zonaManchas.GetChild(0).gameObject);

            manchasRestantesEnEstePlato--;
            puntuacion += puntosPorMancha;
            ActualizarInterfaz();

            if (manchasRestantesEnEstePlato <= 0)
            {
                platosContador++;
                ActualizarInterfaz();
                Invoke("GenerarNuevasManchas", 0.1f);
            }
        }
    }

    void FinalizarJuego()
    {
        if (!juegoActivo) return;
        juegoActivo = false;
        if (musicaJuego != null) musicaJuego.Stop();
        if (panelFinal != null) panelFinal.SetActive(true);
        if (puntosFinalesTexto != null)
        {
            puntosFinalesTexto.text = "<color=red>¡TIEMPO AGOTADO!</color>\n\nPLATOS: " + platosContador + "\nPUNTOS: " + puntuacion;
        }
    }

    void ActualizarInterfaz()
    {
        if (scoreText) scoreText.text = "PUNTOS: " + puntuacion;
        if (platosTotalesText) platosTotalesText.text = "PLATOS: " + platosContador;
    }
}
