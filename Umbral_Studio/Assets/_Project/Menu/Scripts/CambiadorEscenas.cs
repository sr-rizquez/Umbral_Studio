using UnityEngine;
using UnityEngine.SceneManagement;
public class CambiadorEscenas : MonoBehaviour
{
    public void EmpezarModoHistoria() 
    { 
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Equilibrio"); 
    }
    public void JugarEquilibrio() 
    { 
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Equilibrio"); 
    }
    public void JugarSaltaObjetos() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("SaltaObjetos"); 
    }
    public void JugarBuscaYDestruye() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("BuscaDestruye"); 
    }
    public void JugarLavaPlatos() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Dishwashes"); 
    }
    public void JugarRCPComensal() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("RCPComensal"); 
    }
    public void VolverAlMenu() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Menu_version"); 
    }
    public void UnderConstruction() 
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("UnderConstruction"); 
    }
}