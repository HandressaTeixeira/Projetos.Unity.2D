using Assets.Scripts.Util;
using Assets.Scripts.Enumeradores;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class InfoQRCode : MonoBehaviour
{

    [SerializeField]
    private Text _txtCaminho;

    void Start()
    {
        _txtCaminho.text = Geral.PathUploadQRCode();

        Application.OpenURL(Geral.PathUploadQRCode());        
    }

    #region Métodos publicos

    #region Eventos Click

    /// <summary>
    /// Evento de click do botão Voltar - Volta para cena de QRCode
    /// </summary>
    public void OnClickVoltar()
    {
        SceneManager.LoadScene(Cenas.QRCode.ToString());
    }

    #endregion

    #endregion

}
