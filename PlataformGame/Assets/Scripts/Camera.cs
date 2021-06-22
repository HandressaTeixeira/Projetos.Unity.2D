using UnityEngine;

public class Camera : MonoBehaviour
{
    /// <summary>
    /// Velocidade para movimentação da camera
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// Posição do alvo da camera, que é o player, assim a camera segue o player
    /// </summary>
    public Transform target;

    /// <summary>
    /// Tempo "suave" para movimentação da camera
    /// </summary>
    public Vector2 smoothTime;

    /// <summary>
    /// Limite máximo no eixo x e y
    /// </summary>
    public Vector2 maxLimit;

    /// <summary>
    /// Limite minimo no eixo x e y
    /// </summary>
    public Vector2 minLimit;

    void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.y);
    }

    void Update()
    {
        //Se o player existir
        if(target != null)
        {
            //Mathf.SmoothDamp faz um movimento suave
            float posX = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTime.x);
            float posY = Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smoothTime.y);

            //Posição Z é sempre a mesma, geralmente -10 na unity
            transform.position = new Vector3(posX, posY, transform.position.z);

            //Define o limite maximo e minimo da camera
            transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minLimit.x, maxLimit.x),
                    Mathf.Clamp(transform.position.y, minLimit.y, maxLimit.y),
                    transform.position.z
                );
        }
    }
}
