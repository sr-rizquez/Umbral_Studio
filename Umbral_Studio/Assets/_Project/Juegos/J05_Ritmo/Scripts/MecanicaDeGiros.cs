using UnityEngine;
using UnityEngine.UI;

public class MecanicaDeGiros : MonoBehaviour
{
    public RectTransform circuloFijo;
    public RectTransform meta;
    public RectTransform corredor;
    public Button boton;

    public Text Tempo;   // Texto del temporizador
    public Text Puntos;  // Texto de la puntuación

    public float radioCirculo = 200f;
    public float velocidadCorredor = 90f;
    public float tolerancia = 30f;
    public float tiempoDeJuego = 15f;

    private float anguloMeta;
    private float anguloCorredor;
    private bool juegoActivo = true;
    private int puntuacion = 0;
    private float tiempoRestante;

    void Start()
    {
        if (boton != null)
            boton.onClick.AddListener(Click);

        anguloMeta = Random.Range(0f, 360f);
        anguloCorredor = Random.Range(0f, 360f);

        Posicionar(meta, anguloMeta);
        Posicionar(corredor, anguloCorredor);

        tiempoRestante = tiempoDeJuego;

        ActualizarPuntos();
        ActualizarTiempo();
    }

    void Update()
    {
        if (!juegoActivo) return;

        // Movimiento del corredor
        anguloCorredor += velocidadCorredor * Time.deltaTime;

        if (anguloCorredor >= 360f)
            anguloCorredor -= 360f;

        Posicionar(corredor, anguloCorredor);

        // Temporizador
        tiempoRestante -= Time.deltaTime;
        ActualizarTiempo();

        if (tiempoRestante <= 0f)
        {
            juegoActivo = false;
            boton.interactable = false;

            if (Tempo != null)
                Tempo.text = "Tiempo: 0";

            if (Puntos != null)
                Puntos.text = "¡FIN! Puntos: " + puntuacion;

            Debug.Log("Fin del juego por tiempo");
        }
    }

    void Posicionar(RectTransform elemento, float angulo)
    {
        float rad = angulo * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radioCirculo;
        float y = Mathf.Sin(rad) * radioCirculo;

        elemento.anchoredPosition = new Vector2(x, y);
    }

    void Click()
    {
        if (!juegoActivo) return;

        float diff = Mathf.Abs(anguloMeta - anguloCorredor);

        if (diff <= tolerancia || diff >= 360f - tolerancia)
        {
            puntuacion++;
            ActualizarPuntos();

            anguloMeta = Random.Range(0f, 360f);
            Posicionar(meta, anguloMeta);
        }
    }

    void ActualizarPuntos()
    {
        if (Puntos != null)
            Puntos.text = "Puntos: " + puntuacion;
    }

    void ActualizarTiempo()
    {
        if (Tempo != null)
            Tempo.text = "Tiempo: " + Mathf.Ceil(tiempoRestante).ToString();
    }
}