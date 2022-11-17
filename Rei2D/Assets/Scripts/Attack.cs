using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Propriedades publicas
    /// <summary>
    /// Velocidade do ataque
    /// </summary>
    public float speed;

    /// <summary>
    /// Tempo para destruir o objeto de ataque
    /// </summary>
    public float timeDestroy;
    #endregion

    void Start()
    {
        //Destroi o objeto para que não fique fixo na tela
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
