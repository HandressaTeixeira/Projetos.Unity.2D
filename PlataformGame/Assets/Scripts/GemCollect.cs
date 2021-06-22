using UnityEngine;

public class GemCollect : MonoBehaviour
{
    /// <summary>
    /// Musica dos diamantes
    /// </summary>
    public AudioClip fxCollect;

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Verifica se a tag que esta colidindo � do Player
        //Assim os inimigos n�o coletam o diamante
        if (collider.CompareTag("Player"))
        {
            GameManager.instance.score++;
            //Executa o som da Gema
            SoundManager.instance.PlayFxGemCollect(fxCollect);
            //Destroi o diamante
            Destroy(gameObject);
        } 
    }
}
