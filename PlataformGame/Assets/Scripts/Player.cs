using Assets.Scripts.Enumeradores;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Velocidade que o player se mover
    /// </summary>
    public float speed;

    /// <summary>
    /// Quantidade de for�a do pulo
    /// </summary>
    public int jumpForce;

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
    /// Booleano que indica se o player esta pulando
    /// </summary>
    private bool jumping;

    /// <summary>
    /// Boleano que indica se o personagem esta virado para a direita
    /// </summary>
    private bool facingRight = true;

    /// <summary>
    /// Indica se o player esta vivo
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// Indica se o player completou a fase chegando na placa Exit
    /// </summary>
    private bool levelComplete = false;

    /// <summary>
    /// Indica se o tempo terminou
    /// </summary>
    private bool timeOver = false;

    /// <summary>
    /// Objeto Rigidbody 2D do player
    /// </summary>
    private Rigidbody2D rb2D;

    /// <summary>
    /// Compontente Animator para manipular a anima��o
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Musica do player ganhando
    /// </summary>
    public AudioClip fxWin;

    /// <summary>
    /// Musica do player morrendo
    /// </summary>
    public AudioClip fxDie;

    /// <summary>
    /// Musica do player pulando
    /// </summary>
    public AudioClip fxJump;

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
        //OverlapCircle espera a posi��o do toque no ch�o, o raio da cria��o do circulo
        grounded = Physics2D.OverlapCircle(groundCheck.position, radiusOverlapCircle, layerGround);

        //GetButtonDown faz com que o bot�o Cima funcione somente com um clique, n�o valendo clicar e segurar
        //Se adicionar o comando de pulo em FixedUpdate o jogo fica com delay no pulo
        if (Input.GetButtonDown("Jump") && grounded)
        {
            //Comandos de pulo
            jumping = true;

            //Verifica se esta vivo para que n�o fa�a o barulho de pulo ao iniciar a fase
            if (isAlive && !levelComplete)
                SoundManager.instance.PlayFxPlayer(fxJump);
        }

        //Se o tempo acabar o player morre
        if (((int)GameManager.instance.time <= 0) && !timeOver)
        {
            timeOver = true;
            PlayerDie();
        }

        PlayerAnimations();
    }

    /// <summary>
    /// M�todo nativo do Unity
    /// Update para objetos com f�sica (como Rigidbody 2D)
    /// </summary>
    private void FixedUpdate()
    {
        //Verifica se o jogador esta vivo e se n�o completou a fase
        if (isAlive && !levelComplete)
        {
            #region Velocidade do player durante o pulo

            /*Move obtem o o lado do personagem com a velocidade. 
             * Esquerda: velocidade negativa, Direita: velocidade positiva*/
            float move = Input.GetAxis("Horizontal");

            //Velocity espera x e y. A velocidade x deve ser o move vezes a velocidade em speed.
            //A velocidade y deve ser a velocidade atual para que n�o interrompa o movimento do player.
            rb2D.velocity = new Vector2(move * speed, rb2D.velocity.y);
            #endregion

            #region Verifica se o lado esta diferente para mudar o lado
            if ((move < 0 && facingRight) || (move > 0 && !facingRight))
                Flip();
            #endregion

            #region Configura��o da for�a do pulo

            //Aplicar for�a no objeto do personagem
            if (jumping)
            {
                //Eixo X = horizontal, Eixo Y = Vertical
                //No eixo x n�o adicionar for�a para que n�o some com a for�a do pulo fazendo o player pular longe
                rb2D.AddForce(new Vector2(0f, jumpForce));
                jumping = false;
            }

            #endregion

        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

    }

    /// <summary>
    /// Anima��es do player
    /// </summary>
    void PlayerAnimations()
    {

        #region Configura��o das anima��es Correr (Run), Parado (Idle), Morte (Die) e Pulo (Jump) do player
        //Verifica se o personagem completou a fase chegando na placa Exit
        if (levelComplete)
            anim.Play("Celebrate");
        //Verifica se o personagem ta vivo
        else if (!isAlive)
            anim.Play("Die");
        //Verifica se o player esta correndo
        else if (grounded && rb2D.velocity.x != 0)
            anim.Play("Run");
        //Anima��o Idle se o player parado
        else if (grounded && rb2D.velocity.x == 0)
            anim.Play("Idle");
        //Se n�o chama Anima��o de pulo
        else if (!grounded)
            anim.Play("Jump");

        #endregion

    }

    /// <summary>
    /// M�todo que vira o personagem conforme o lado que estiver andando
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;

        //localScale.x segue a regra matematica de sinais, assim altera entre 1 e -1 para mudar o lado do player
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Controle de colis�o do player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Configura��o de morte do player
        if (collision.gameObject.CompareTag("Enemy"))
            PlayerDie();

    }

    /// <summary>
    /// M�todo que muda o status do player para morto e para as colis�es com o inimigo
    /// </summary>
    void PlayerDie()
    {
        //Indica Player como morto
        isAlive = false;

        //Ignora a colis�o Enemy X Player que esta na Matriz de Colis�o de Camada (Layer Collision Matrix, em Project Settings, Physics 2D)
        //Assim o inimigo n�o fica colidindo ou empurrando o player depois de morto
        Physics2D.IgnoreLayerCollision(9, 10);

        SoundManager.instance.PlayFxPlayer(fxDie);
    }

    /// <summary>
    /// Colis�o com objetos que possuem Is Trigger marcado.
    /// Eventos tratados: colis�o com placa Exit
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Exit"))
        {
            levelComplete = true;

            SoundManager.instance.PlayFxPlayer(fxWin);
        }
    }

    /// <summary>
    /// M�todo chamado no ultimo frame da anima��o Die (na Unity -> Animation -> Die -> AnimationEvent)
    /// para apresentar o overlay Die ou Lose
    /// </summary>
    public void DieAnimationFinished()
    {
        if (timeOver)
            GameManager.instance.SetOverlay(StatusJogo.LOSE);
        else
            GameManager.instance.SetOverlay(StatusJogo.DIE);
    }

    /// <summary>
    /// M�todo chamado no ultimo frame da anima��o Celebrate para apresentar o overlay Win
    /// </summary>
    public void CelebrateAnimationFinished()
    {
        GameManager.instance.SetOverlay(StatusJogo.WIN);
    }
}
