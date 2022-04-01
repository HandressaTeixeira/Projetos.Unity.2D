using Assets.Scripts.Enumeradores;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private Text _txtVersao;

    void Start()
    {
        _txtVersao.text = $"Vers�o: {Application.version}";
    }

    #region M�todos publicos

    #region Eventos Click

    /// <summary>
    /// Evento de click do bot�o QRCode
    /// </summary>
    public void OnClickQRCode()
    {
        SceneManager.LoadScene(Cenas.QRCode.ToString());
    }

    /// <summary>
    /// Evento de click do bot�o Pol�tica de Privacidade
    /// </summary>
    public void OnClickPoliticaPrivacidade()
    {
        Application.OpenURL("https://sites.google.com/view/sistersdev-politicas/politicas-de-privacidade/GeradorFacilQRCode");
        //SceneManager.LoadScene(Cenas.PoliticaPrivacidade.ToString());

    }

    /// <summary>
    /// Evento de click do bot�o Sair
    /// </summary>
    public void OnClickSair()
    {
        Application.Quit();
    }
    #endregion

    #endregion

}
