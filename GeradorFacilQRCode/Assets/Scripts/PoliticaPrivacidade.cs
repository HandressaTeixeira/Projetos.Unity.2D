using Assets.Scripts.Enumeradores;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PoliticaPrivacidade : MonoBehaviour
{

    void Start()
    {
        
    }

    #region M�todos publicos

    #region Eventos Click

    /// <summary>
    /// Evento de click do bot�o Voltar
    /// </summary>
    public void OnClickVoltar()
    {
        SceneManager.LoadScene(Cenas.Menu.ToString());
    }

    #endregion

    #endregion

}
