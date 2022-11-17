using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    #region Propriedades públicas

    /// <summary>
    /// Velocidade do player
    /// </summary>
    public float speed;

    /// <summary>
    /// Saúde do player
    /// </summary>
    public int health;

    /// <summary>
    /// Transform para controlar onde a cobra encosta
    /// </summary>
    public Transform wallCheck;
    #endregion

    #region Métodos privados

    /// <summary>
    /// Indica se esta olhando para a direita. É possivel simular com o Flip X
    /// </summary>
    private bool facingRight = true;

    /// <summary>
    /// Indica se esta tocando a parede
    /// </summary>
    private bool tochedWall = false;

    /// <summary>
    /// Sprite Renderer do player
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// Rigidbody 2D do player
    /// </summary>
    private Rigidbody2D rb2D;
    #endregion

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        tochedWall = Physics2D.Linecast
            (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (tochedWall)
            Flip();

    }

    void FixedUpdate()
    {
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }

    /// <summary>
    /// Faz a cobra mudar de lado
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3
            (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }
}
