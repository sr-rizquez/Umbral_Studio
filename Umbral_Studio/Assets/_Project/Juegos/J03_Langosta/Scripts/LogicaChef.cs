using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // Necesario para gestionar la lista de posiciones

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
    private bool cuentaAtrasActiva = true;

    // --- SISTEMA ANTISUPERPOSICIÓN Y NO REPETICIÓN ---
    private static List<int> indicesOcupados = new List<int>();
    private int miIndiceActual = -1;

    void Start()
    {
        cronometro = tiempoLimite;
        if (botonReiniciar != null) botonReiniciar.SetActive(false);

        // El chef empieza oculto durante la cuenta atrás
        ToggleChef(false);
        StartCoroutine(CuentaAtrasInicial());
    }

    IEnumerator CuentaAtrasInicial()
    {
        float tiempo = 5f;
        while (tiempo > 0)
        {
            if (textoUI != null)
                textoUI.text = "<color=yellow>PREPÁRATE</color>\n" + Mathf.Ceil(tiempo).ToString();
            yield return new WaitForSeconds(1f);
            tiempo--;
        }

        if (textoUI != null) textoUI.text = "¡YA!";
        yield return new WaitForSeconds(0.5f);

        // Activamos el juego
        cuentaAtrasActiva = false;
        ToggleChef(true);
        MoverChefAleatorio();
        if (musicaFondo != null && !musicaFondo.isPlaying) musicaFondo.Play();
    }

    void Update()
    {
        // Bloquea el juego si estamos en la cuenta atrás de 5s
        if (cuentaAtrasActiva) return;

        Ray mirada = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit golpe;

        if (!juegoTerminado)
        {
            cronometro -= Time.deltaTime;

            // Detección por mirada (SphereCast)
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
            // Lógica de reinicio por mirada
            if (Physics.Raycast(mirada, out golpe, 100f))
            {
                if (golpe.transform.gameObject == botonReiniciar)
                {
                    contadorMirada += Time.deltaTime;
                    if (textoUI != null)
                        textoUI.text = "<color=green>REINICIANDO...</color>\n" + (tiempoBoton - contadorMirada).ToString("F1") + "s";

                    if (contadorMirada >= tiempoBoton)
                    {
                        indicesOcupados.Clear(); // Limpieza para la nueva partida
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    contadorMirada = 0f;
                    ActualizarTextoFinal();
                }
            }
        }
    }

    void MoverChefAleatorio()
    {
        if (puntosAparicion.Length < 2) return;

        // Guardamos donde estábamos para no repetir el mismo sitio exacto
        int indiceAnterior = miIndiceActual;

        // Liberamos el punto que ocupábamos
        if (miIndiceActual != -1) indicesOcupados.Remove(miIndiceActual);

        int nuevoIndice = -1;
        bool encontrado = false;
        int intentos = 0;

        // Buscamos un punto que no esté ocupado por otro chef Y que no sea el anterior
        while (!encontrado && intentos < 20)
        {
            int candidato = Random.Range(0, puntosAparicion.Length);

            if (!indicesOcupados.Contains(candidato) && candidato != indiceAnterior)
            {
                nuevoIndice = candidato;
                encontrado = true;
            }
            intentos++;
        }

        // Caso de emergencia si fallan los intentos (forzar cambio de sitio)
        if (!encontrado)
        {
            do
            {
                nuevoIndice = Random.Range(0, puntosAparicion.Length);
            } while (nuevoIndice == indiceAnterior && puntosAparicion.Length > 1);
        }

        miIndiceActual = nuevoIndice;
        indicesOcupados.Add(miIndiceActual);
        transform.position = puntosAparicion[miIndiceActual].position;
    }

    void EliminarChef()
    {
        eliminaciones++;
        if (eliminaciones >= maxEliminaciones) FinalizarJuego(true);
        else MoverChefAleatorio();
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

    void ToggleChef(bool estado)
    {
        if (GetComponent<Renderer>() != null) GetComponent<Renderer>().enabled = estado;
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = estado;
    }

    void FinalizarJuego(bool ganado)
    {
        juegoTerminado = true;
        contadorMirada = 0f;
        if (musicaFondo != null) musicaFondo.Stop();
        if (botonReiniciar != null) botonReiniciar.SetActive(true);

        indicesOcupados.Remove(miIndiceActual); // Liberamos posición al terminar
        ToggleChef(false);
        ActualizarTextoFinal();
    }
}
