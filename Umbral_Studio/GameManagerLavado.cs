using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; 

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
    public GameObject manchaPrefab;
    public RectTransform zonaManchas;
    public float tiempoInicial = 15f;

    [Header("Audio")]
    public AudioSource musicaJuego;

    private float tiempoRestante;
    private int puntuacion = 0;
    private int platosContador = 0;
    private int manchasRestantesEnEstePlato;
    private bool juegoActivo = false;

    void Start()
    {
        if (timerText == null || scoreText == null || platosTotalesText == null || manchaPrefab == null || zonaManchas == null)
        {
            Debug.LogError("¡ERROR! Revisa los huecos en el Inspector del _Brain.");
            return;
        }

        if (panelFinal != null) panelFinal.SetActive(false);

        tiempoRestante = tiempoInicial;
        puntuacion = 0;
        platosContador = 0;

        ActualizarInterfaz();
        juegoActivo = true;

        if (musicaJuego != null)
        {
            musicaJuego.Stop();
            musicaJuego.Play();
        }

        GenerarNuevasManchas();
    }

    void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0)
        {
            FinalizarJuego();
        }
        timerText.text = "TIEMPO: " + Mathf.Ceil(tiempoRestante).ToString();

        // --- NUEVO SISTEMA DE ENTRADA ---
        // Detecta la tecla Espacio en PC O cualquier toque en la pantalla del móvil
        bool tecladoEspacio = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool toquePantalla = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;

        if (tecladoEspacio || toquePantalla)
        {
            EliminarUnaMancha();
        }
    }

    void GenerarNuevasManchas()
    {
        foreach (Transform hijo in zonaManchas) { Destroy(hijo.gameObject); }

        manchasRestantesEnEstePlato = Random.Range(3, 6);

        for (int i = 0; i < manchasRestantesEnEstePlato; i++)
        {
            GameObject m = Instantiate(manchaPrefab, zonaManchas);
            float x = Random.Range(-zonaManchas.rect.width / 2.5f, zonaManchas.rect.width / 2.5f);
            float y = Random.Range(-zonaManchas.rect.height / 2.5f, zonaManchas.rect.height / 2.5f);
            m.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            Button btn = m.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(() => EliminarUnaMancha());
        }
    }

    void EliminarUnaMancha()
    {
        if (!juegoActivo || manchasRestantesEnEstePlato <= 0) return;

        if (zonaManchas.childCount > 0)
        {
            Destroy(zonaManchas.GetChild(0).gameObject);
        }

        manchasRestantesEnEstePlato--;
        puntuacion += 10;
        ActualizarInterfaz();

        if (manchasRestantesEnEstePlato <= 0)
        {
            platosContador++;
            ActualizarInterfaz();
            GenerarNuevasManchas();
        }
    }

    void FinalizarJuego()
    {
        tiempoRestante = 0;
        juegoActivo = false;

        if (musicaJuego != null)
        {
            musicaJuego.Stop();
            musicaJuego.loop = false;
            musicaJuego.enabled = false;
        }

        if (panelFinal != null)
        {
            panelFinal.SetActive(true);
            if (puntosFinalesTexto != null)
            {
                puntosFinalesTexto.text = "¡TIEMPO AGOTADO!\nPLATOS LIMPIOS: " + platosContador + "\nPUNTOS TOTALES: " + puntuacion;
            }
        }
    }

    void ActualizarInterfaz()
    {
        scoreText.text = "PUNTOS: " + puntuacion;
        platosTotalesText.text = "PLATOS: " + platosContador;
    }
}