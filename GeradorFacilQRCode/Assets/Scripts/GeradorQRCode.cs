using Assets.Scripts.Util;
using Assets.Scripts.Enumeradores;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using UnityEngine.Android;

public class GeradorQRCode : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageQRCode;

    [SerializeField]
    private InputField _inputQRCode;

    [SerializeField]
    private Text _txtMensagem;

    private Texture2D _texture;

    void Start()
    {
        _texture = new Texture2D(256, 256);
    }

    #region Métodos publicos

    #region Eventos Click

    /// <summary>
    /// Evento de click do botão Gerar
    /// </summary>
    public void OnClickEncode()
    {
        try
        {
            EncodeTextToQRCode();
        }
        catch (Exception ex)
        {
            MostrarMensagem("Erro ao gerar o QRCode.");
            throw ex;
        }
    }

    /// <summary>
    /// Evento de click do botão Salvar
    /// </summary>
    public void OnClickSalvar()
    {
        try
        {
            if (_rawImageQRCode.texture == null)
            {
                MostrarMensagem("Gere um QRCode para salva-lo.");
                return;
            }
            var texture = (Texture2D)_rawImageQRCode.texture;

            var bytesJPG = texture.EncodeToJPG();
            var _caminho = Geral.PathUploadQRCode();

            if (!Directory.Exists(_caminho))
            {
                Directory.CreateDirectory(_caminho);
            }

            var fileName = $"QRCode_{ Guid.NewGuid()}.jpg";
            var path = $"{_caminho}{fileName}";

            File.WriteAllBytes(path, bytesJPG);
            
            MostrarMensagem("QRCode salvo com sucesso!");
        }
        catch (Exception ex)
        {
            MostrarMensagem("Erro ao salvar o QRCode.");
            throw ex;
        }
    }

    /// <summary>
    /// Evento de click do botão Voltar - Volta para cena de menu
    /// </summary>
    public void OnClickVoltar()
    {
        SceneManager.LoadScene(Cenas.Menu.ToString());
    }

    /// <summary>
    /// Evento de click do botão Info - Chama cena InfoQRCode
    /// </summary>
    public void OnClickInfo()
    {
        SceneManager.LoadScene(Cenas.InfoQRCode.ToString());
    }
    #endregion

    #endregion

    #region Métodos privados

    /// <summary>
    /// Converte Color para Texture2D
    /// </summary>
    private void EncodeTextToQRCode()
    {
        if (string.IsNullOrWhiteSpace(_inputQRCode.text))
        {
            MostrarMensagem("Digite algo no campo para gerar QRCode!");
            return;
        }

        string textWrite = Geral.SemAcento(_inputQRCode.text);

        Color32[] _color32 = Encode(textWrite, _texture.width, _texture.height);

        _texture.SetPixels32(_color32);
        _texture.Apply();

        _rawImageQRCode.texture = _texture;
    }

    /// <summary>
    /// Gerador de QRCode
    /// </summary>
    /// <param name="texto">Texto</param>
    /// <param name="width">Largura</param>
    /// <param name="height">Altura</param>
    /// <returns></returns>
    private Color32[] Encode(string texto, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter()
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(texto);
    }

    /// <summary>
    /// Preenche TxtMensagem para exibir mensagem na tela
    /// </summary>
    /// <param name="msg"></param>
    private void MostrarMensagem(string msg)
    {
        _txtMensagem.text = msg;
        Invoke(nameof(LimparMensagem), 5f);
    }

    /// <summary>
    /// Limpa mensagem do TxtMensagem
    /// </summary>
    private void LimparMensagem()
    {
        _txtMensagem.text = string.Empty;
    }

    #endregion
}
