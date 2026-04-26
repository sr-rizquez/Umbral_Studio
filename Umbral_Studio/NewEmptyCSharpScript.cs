using UnityEngine;
using UnityEngine.UI;

public class ControladorJuego : MonoBehaviour
{
    // Huecos para Unity
    public GameObject chefPrefab;
    public Transform[] puntosSpawn;
    public Image reticula;

    private int puntos = 0;

    void Start()
    {
        AparecerChef();
    }

    void Update()
    {
        RaycastHit hit;
        // Lanzamos un rayo desde el centro de la cámara
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            // AQUÍ ESTÁ EL CAMBIO: "chef" en minúsculas
            if (hit.transform.CompareTag("chef"))
            {
                reticula.color = Color.green; // Se pone verde

                // Si pulsas la pantalla o el botón del visor
                if (Input.GetMouseButtonDown(0))
                {
                    EliminarChef();
                }
            }
            else
            {
                reticula.color = Color.white; // Vuelve a blanco si no miras al chef
            }
        }
    }

    void EliminarChef()
    {
        puntos += 10;
        Debug.Log("¡Chef capturado! Puntos: " + puntos);
        AparecerChef(); // El chef se mueve a otro punto
    }

    void AparecerChef()
    {
        // Elige un punto al azar de la lista
        int indiceAleatorio = Random.Range(0, puntosSpawn.Length);
        // Mueve al Chef a esa posición
        chefPrefab.transform.position = puntosSpawn[indiceAleatorio].position;
    }
}
