using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicaChef : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public Transform[] puntosAparicion;
    public TextMeshProUGUI textoUI;
    public GameObject botonReiniciar;
    public AudioSource musicaFondo;

    [Header("Ajustes de Juego")]
    public float tiempoLimite = 20f;
    public int maxEliminaciones = 5;

    [Header("Tiempos de Mirada")]
    public float tiempoChef = 1.5f;
    public float tiempoBoton = 1f;

    private float contadorMirada = 0f;
    private int eliminaciones = 0;
    private float cronometro;
    private bool juegoTerminado = false;

    void Start()
    {
        cronometro = tiempoLimite;
        if (botonReiniciar != null) botonReiniciar.SetActive(false);
        if (musicaFondo != null && !musicaFondo.isPlaying) musicaFondo.Play();
        MoverChefAleatorio();
    }

    void Update()
    {
        Ray mirada = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit golpe;

        if (!juegoTerminado)
        {
            cronometro -= Time.deltaTime;
            // SphereCast con radio 0.5 para detectar al chef
            if (Physics.SphereCast(mirada, 0.5f, out golpe, 100f) && golpe.transform == this.transform)
            {
                contadorMirada += Time.deltaTime;
                if (contadorMirada >= tiempoChef)
                {
                    EliminarChef();
                    contadorMirada = 0f;
                }
            }
            else { contadorMirada = 0f; }

            ActualizarInterfaz();
            if (cronometro <= 0) FinalizarJuego(false);
        }
        else
        {
            // --- LÓGICA DE REINICIO MEJORADA ---
            // Usamos un Raycast simple (más preciso) para el botón
            if (Physics.Raycast(mirada, out golpe, 100f))
            {
                // Para depurar: esto te dirá en la consola qué estás mirando exactamente
                Debug.Log("Mirada puesta en: " + golpe.transform.name);

                if (golpe.transform.gameObject == botonReiniciar)
                {
                    contadorMirada += Time.deltaTime;
                    if (textoUI != null)
                        textoUI.text = "<color=green>REINICIANDO...</color>\n" + (tiempoBoton - contadorMirada).ToString("F1") + "s";

                    if (contadorMirada >= tiempoBoton)
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    contadorMirada = 0f;
                    ActualizarTextoFinal();
                }
            }
        }
    }

    void ActualizarInterfaz()
    {
        if (textoUI != null)
        {
            textoUI.text = "TIEMPO: " + Mathf.Ceil(cronometro).ToString() + "s\n" +
                           "CHEFS: " + eliminaciones + "/" + maxEliminaciones + "\n" +
                           (contadorMirada > 0 ? "FIJANDO: " + contadorMirada.ToString("F1") : "");
        }
    }

    void ActualizarTextoFinal()
    {
        if (textoUI != null)
        {
            textoUI.text = (cronometro <= 0 ? "¡TIEMPO AGOTADO!" : "¡GANASTE!") +
                           "\nFinal: " + eliminaciones + " chefs\n<color=yellow>MIRA EL BOTÓN</color>";
        }
    }

    void EliminarChef()
    {
        eliminaciones++;
        if (eliminaciones >= maxEliminaciones) FinalizarJuego(true);
        else MoverChefAleatorio();
    }

    void MoverChefAleatorio()
    {
        if (puntosAparicion.Length > 0)
        {
            int indice = Random.Range(0, puntosAparicion.Length);
            transform.position = puntosAparicion[indice].position;
        }
    }

    void FinalizarJuego(bool ganado)
    {
        juegoTerminado = true;
        contadorMirada = 0f;
        if (musicaFondo != null) musicaFondo.Stop();
        if (botonReiniciar != null) botonReiniciar.SetActive(true);
        ActualizarTextoFinal();

        // Desactivamos render y collider del Chef
        if (GetComponent<Renderer>() != null) GetComponent<Renderer>().enabled = false;
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
    }
}
