using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /// <summary>
    /// Velocidade que o inimigo se move
    /// </summary>
    public float speed;

    /// <summary>
    /// Quando o player esta tocando o ch�o
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Camada que pertence ao ch�o para validar groundCheck
    /// </summary>
    public LayerMask layerGround;

    /// <summary>
    /// Raio para cria��o do circulo OverlapCircle
    /// </summary>
    public float radiusOverlapCircle;

    /// <summary>
    /// Booleano que indica se esta tocando o ch�o. O Transform GroundCheck � o circulo invisivel que muda grounded
    /// para true quando toca o ch�o
    /// </summary>
    private bool grounded;

    /// <summary>
    /// Boleano que indica se o personagem esta virado para a direita
    /// </summary>
    private bool facingRight = true;

    /// <summary>
    /// Objeto Rigidbody 2D do player
    /// </summary>
    private Rigidbody2D rb2D;

    /// <summary>
    /// Compontente Animator para manipular a anima��o
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Indica se o inimigo esta vis�vel na tela
    /// </summary>
    private bool isVisible = false;

    /// <summary>
    /// M�todo nativo do Unity
    /// Inicio do script
    /// </summary>
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// M�todo nativo do Unity
    /// M�todo que � executado 30 vezes por segundo para atualizar/validar uma a��o do player
    /// </summary>
    void Update()
    {
        //OverlapCircle � um circulo invisivel em baixo do player que verifica se o que esta colidindo a baixo dele
        //OverlapCircle espera a posi��o do toque no ch�om, o raio da cria��o do circulo
        grounded = Physics2D.OverlapCircle(groundCheck.position, radiusOverlapCircle, layerGround);

        if (!grounded)
            Flip();
    }

    /// <summary>
    /// M�todo nativo do Unity
    /// Update para objetos com f�sica (como Rigidbody 2D)
    /// </summary>
    private void FixedUpdate()
    {

        //Se o inimigo estiver visivel deve se mover
        if (isVisible)
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        //Se n�o deve ficar parado
        else
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);

    }

    /// <summary>
    /// M�todo que vira o personagem conforme o lado que estiver andando
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;

        //localScale.x segue a regra matematica de sinais, assim altera entre 1 e -1 para mudar o lado do player
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        speed *= -1; //Necess�rio porque o inimigo se movimenta automaticamente com a ajuda do GroundCheck
    }

    /// <summary>
    /// M�todo nativo do Unity Responsavel por controllar um personagem/objeto quando est� visivel
    /// </summary>
    private void OnBecameVisible()
    {
        //Chama o m�todo de anima��o Correr ap�s um delay de 3 segundos
        Invoke("MoveEnemy", 3f);
    }

    /// <summary>
    /// M�todo nativo do Unity Responsavel por controllar um personagem/objeto quando est� invisivel
    /// </summary>
    private void OnBecameInvisible()
    {
        //Chama o m�todo de anima��o Parado ap�s um delay de 3 segundos
        Invoke("StopEnemy", 3f);
    }

    /// <summary>
    /// M�todo que inicia a anima��o Correr (Run)
    /// </summary>
    void MoveEnemy()
    {
        isVisible = true;
        anim.Play("Run");
    }

    /// <summary>
    /// M�todo que inicia a anima��o Parado e Respirando (Idle)
    /// </summary>
    void StopEnemy()
    {
        isVisible = false;
        anim.Play("Idle");
    }
}
