using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiadorEscenas : MonoBehaviour
{
    public void EmpezarModoHistoria() { SceneManager.LoadScene("Equilibrio"); }
    public void JugarEquilibrio() { SceneManager.LoadScene("Equilibrio"); }
    public void JugarSaltaObjetos() { SceneManager.LoadScene("SaltaObjetos"); }
    public void JugarBuscaYDestruye() { SceneManager.LoadScene("BuscaDestruye"); }
    public void JugarLavaPlatos() { SceneManager.LoadScene("Dishwashes"); }
    public void JugarRCPComensal() { SceneManager.LoadScene("RCPComensal"); }
    public void VolverAlMenu() { SceneManager.LoadScene("Menu_version"); }
    public void UnderConstruction() { SceneManager.LoadScene("UnderConstruction"); }
}