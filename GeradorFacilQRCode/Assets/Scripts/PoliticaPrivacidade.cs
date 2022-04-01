using Assets.Scripts.Enumeradores;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PoliticaPrivacidade : MonoBehaviour
{

    void Start()
    {
        
    }

    #region Métodos publicos

    #region Eventos Click

    /// <summary>
    /// Evento de click do botão Voltar
    /// </summary>
    public void OnClickVoltar()
    {
        SceneManager.LoadScene(Cenas.Menu.ToString());
    }

    #endregion

    #endregion

}
