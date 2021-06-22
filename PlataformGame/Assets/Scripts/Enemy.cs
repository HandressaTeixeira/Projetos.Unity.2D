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
    /// Quando o player esta tocando o chão
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Camada que pertence ao chão para validar groundCheck
    /// </summary>
    public LayerMask layerGround;

    /// <summary>
    /// Raio para criação do circulo OverlapCircle
    /// </summary>
    public float radiusOverlapCircle;

    /// <summary>
    /// Booleano que indica se esta tocando o chão. O Transform GroundCheck é o circulo invisivel que muda grounded
    /// para true quando toca o chão
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
    /// Compontente Animator para manipular a animação
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Indica se o inimigo esta visível na tela
    /// </summary>
    private bool isVisible = false;

    /// <summary>
    /// Método nativo do Unity
    /// Inicio do script
    /// </summary>
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Método nativo do Unity
    /// Método que é executado 30 vezes por segundo para atualizar/validar uma ação do player
    /// </summary>
    void Update()
    {
        //OverlapCircle é um circulo invisivel em baixo do player que verifica se o que esta colidindo a baixo dele
        //OverlapCircle espera a posição do toque no chãom, o raio da criação do circulo
        grounded = Physics2D.OverlapCircle(groundCheck.position, radiusOverlapCircle, layerGround);

        if (!grounded)
            Flip();
    }

    /// <summary>
    /// Método nativo do Unity
    /// Update para objetos com física (como Rigidbody 2D)
    /// </summary>
    private void FixedUpdate()
    {

        //Se o inimigo estiver visivel deve se mover
        if (isVisible)
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        //Se não deve ficar parado
        else
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);

    }

    /// <summary>
    /// Método que vira o personagem conforme o lado que estiver andando
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;

        //localScale.x segue a regra matematica de sinais, assim altera entre 1 e -1 para mudar o lado do player
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        speed *= -1; //Necessário porque o inimigo se movimenta automaticamente com a ajuda do GroundCheck
    }

    /// <summary>
    /// Método nativo do Unity Responsavel por controllar um personagem/objeto quando está visivel
    /// </summary>
    private void OnBecameVisible()
    {
        //Chama o método de animação Correr após um delay de 3 segundos
        Invoke("MoveEnemy", 3f);
    }

    /// <summary>
    /// Método nativo do Unity Responsavel por controllar um personagem/objeto quando está invisivel
    /// </summary>
    private void OnBecameInvisible()
    {
        //Chama o método de animação Parado após um delay de 3 segundos
        Invoke("StopEnemy", 3f);
    }

    /// <summary>
    /// Método que inicia a animação Correr (Run)
    /// </summary>
    void MoveEnemy()
    {
        isVisible = true;
        anim.Play("Run");
    }

    /// <summary>
    /// Método que inicia a animação Parado e Respirando (Idle)
    /// </summary>
    void StopEnemy()
    {
        isVisible = false;
        anim.Play("Idle");
    }
}
