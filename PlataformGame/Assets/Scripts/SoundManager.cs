using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    /// <summary>
    /// Musicas do player
    /// </summary>
    public AudioSource fxPlayer;

    /// <summary>
    /// Musicas dos diamantes
    /// </summary>
    public AudioSource fxGemCollect;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //Faz com que a instancia n�o seja destruida quando passar de fase
        //M�todo util para jogos que precisam continuar com a pontua��o mesmo ap�s mudar de fase
        DontDestroyOnLoad(gameObject);
    }

    public void PlayFxPlayer(AudioClip clip)
    {
        fxPlayer.clip = clip;
        fxPlayer.Play();
    }

    public void PlayFxGemCollect(AudioClip clip)
    {
        fxGemCollect.clip = clip;
        fxGemCollect.Play();
    }
}
