using Assets.Scripts.Enumeradores;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Necessário para utilizar GameManager do jogo
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// Lista de Overlays (You Win, You Lose e You Die)
    /// Ao arrastar as imagens para a lista cuidar para que fique na mesma sequencia que o enumerador
    /// StatusJogo
    /// </summary>
    public Sprite[] overlays;

    /// <summary>
    /// Imagem para ser mostrada no overlay
    /// </summary>
    public Image overlay;

    /// <summary>
    /// Tempo do jogo em texto
    /// </summary>
    public Text timeHud;

    /// <summary>
    /// Pontuação em texto
    /// </summary>
    public Text scoreHud;

    /// <summary>
    /// Tempo do jogo em float
    /// </summary>
    public float time;

    /// <summary>
    /// Pontuação do jogo em int
    /// </summary>
    public int score;

    public StatusJogo status;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //time = 30f;
        score = 0;
        status = StatusJogo.PLAY;
        overlay.enabled = false;
        //false faz com que pare de ignorar as layers 9 e 10
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    void Update()
    {
        if (status == StatusJogo.PLAY)
        {
            //Diminui 1 segundo
            time -= Time.deltaTime;
            int timeInt = (int)time;

            if (timeInt >= 0)
            {
                timeHud.text = $"Time: {timeInt}";
                scoreHud.text = $"Score: {score}";
            }

        }
        else if (Input.GetButtonDown("Jump"))
        {
            if (status == StatusJogo.WIN)
            {
                //Carrega a proxima fase (fase atual mais 1)
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                //Como fazer para o jogo não parar, quando finalizar a fase 2 voltar para a fase 1
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    /// <summary>
    /// Mostra o overlay
    /// </summary>
    public void SetOverlay(StatusJogo parStatus)
    {
        status = parStatus;
        overlay.enabled = true;
        overlay.sprite = overlays[(int)parStatus];
    }
}
