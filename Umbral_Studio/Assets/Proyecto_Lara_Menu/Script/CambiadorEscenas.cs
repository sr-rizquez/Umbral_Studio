using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiadorEscenas : MonoBehaviour
{
    public void EmpezarModoHistoria() { SceneManager.LoadScene("EscenaHistoria1"); }
    public void JugarEquilibrio() { SceneManager.LoadScene("Equilibrio"); }
    public void JugarSaltaObjetos() { SceneManager.LoadScene("SaltaObjetos"); }
    public void JugarBuscaYDestruye() { SceneManager.LoadScene("BuscaDestruye"); }
    public void JugarLavaPlatos() { SceneManager.LoadScene("LavaPlatos"); }
    public void JugarRCPComensal()
    {
        SceneManager.LoadScene("RCPComensal");
    }
}