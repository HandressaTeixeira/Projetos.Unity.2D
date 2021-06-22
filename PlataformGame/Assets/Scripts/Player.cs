using Assets.Scripts.Enumeradores;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Velocidade que o player se mover
    /// </summary>
    public float speed;

    /// <summary>
    /// Quantidade de força do pulo
    /// </summary>
    public int jumpForce;

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
    /// Compontente Animator para manipular a animação
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
        //OverlapCircle espera a posição do toque no chão, o raio da criação do circulo
        grounded = Physics2D.OverlapCircle(groundCheck.position, radiusOverlapCircle, layerGround);

        //GetButtonDown faz com que o botão Cima funcione somente com um clique, não valendo clicar e segurar
        //Se adicionar o comando de pulo em FixedUpdate o jogo fica com delay no pulo
        if (Input.GetButtonDown("Jump") && grounded)
        {
            //Comandos de pulo
            jumping = true;

            //Verifica se esta vivo para que não faça o barulho de pulo ao iniciar a fase
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
    /// Método nativo do Unity
    /// Update para objetos com física (como Rigidbody 2D)
    /// </summary>
    private void FixedUpdate()
    {
        //Verifica se o jogador esta vivo e se não completou a fase
        if (isAlive && !levelComplete)
        {
            #region Velocidade do player durante o pulo

            /*Move obtem o o lado do personagem com a velocidade. 
             * Esquerda: velocidade negativa, Direita: velocidade positiva*/
            float move = Input.GetAxis("Horizontal");

            //Velocity espera x e y. A velocidade x deve ser o move vezes a velocidade em speed.
            //A velocidade y deve ser a velocidade atual para que não interrompa o movimento do player.
            rb2D.velocity = new Vector2(move * speed, rb2D.velocity.y);
            #endregion

            #region Verifica se o lado esta diferente para mudar o lado
            if ((move < 0 && facingRight) || (move > 0 && !facingRight))
                Flip();
            #endregion

            #region Configuração da força do pulo

            //Aplicar força no objeto do personagem
            if (jumping)
            {
                //Eixo X = horizontal, Eixo Y = Vertical
                //No eixo x não adicionar força para que não some com a força do pulo fazendo o player pular longe
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
    /// Animações do player
    /// </summary>
    void PlayerAnimations()
    {

        #region Configuração das animações Correr (Run), Parado (Idle), Morte (Die) e Pulo (Jump) do player
        //Verifica se o personagem completou a fase chegando na placa Exit
        if (levelComplete)
            anim.Play("Celebrate");
        //Verifica se o personagem ta vivo
        else if (!isAlive)
            anim.Play("Die");
        //Verifica se o player esta correndo
        else if (grounded && rb2D.velocity.x != 0)
            anim.Play("Run");
        //Animação Idle se o player parado
        else if (grounded && rb2D.velocity.x == 0)
            anim.Play("Idle");
        //Se não chama Animação de pulo
        else if (!grounded)
            anim.Play("Jump");

        #endregion

    }

    /// <summary>
    /// Método que vira o personagem conforme o lado que estiver andando
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;

        //localScale.x segue a regra matematica de sinais, assim altera entre 1 e -1 para mudar o lado do player
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Controle de colisão do player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Configuração de morte do player
        if (collision.gameObject.CompareTag("Enemy"))
            PlayerDie();

    }

    /// <summary>
    /// Método que muda o status do player para morto e para as colisões com o inimigo
    /// </summary>
    void PlayerDie()
    {
        //Indica Player como morto
        isAlive = false;

        //Ignora a colisão Enemy X Player que esta na Matriz de Colisão de Camada (Layer Collision Matrix, em Project Settings, Physics 2D)
        //Assim o inimigo não fica colidindo ou empurrando o player depois de morto
        Physics2D.IgnoreLayerCollision(9, 10);

        SoundManager.instance.PlayFxPlayer(fxDie);
    }

    /// <summary>
    /// Colisão com objetos que possuem Is Trigger marcado.
    /// Eventos tratados: colisão com placa Exit
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
    /// Método chamado no ultimo frame da animação Die (na Unity -> Animation -> Die -> AnimationEvent)
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
    /// Método chamado no ultimo frame da animação Celebrate para apresentar o overlay Win
    /// </summary>
    public void CelebrateAnimationFinished()
    {
        GameManager.instance.SetOverlay(StatusJogo.WIN);
    }
}
