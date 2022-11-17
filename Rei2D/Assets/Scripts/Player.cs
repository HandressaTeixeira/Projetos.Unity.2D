using UnityEngine;

public class Player : MonoBehaviour
{
    #region Propriedades publicas
    /// <summary>
    /// Velocidade do player
    /// </summary>
    public float speed;

    /// <summary>
    /// For�a do pulo
    /// </summary>
    public int jumpForce;

    /// <summary>
    /// Sa�de do player
    /// </summary>
    public int health;

    /// <summary>
    /// Quantidade de ataques por segundo
    /// </summary>
    public float attackRate;

    /// <summary>
    /// Transform do SpaumAtack
    /// </summary>
    public Transform  transformSpaumAtack;

    /// <summary>
    /// Transform do ground check do player
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Objeto do ataque
    /// </summary>
    public GameObject attackPrefab;

    #endregion

    #region M�todos privados

    /// <summary>
    /// Indica se esta vulneravel
    /// </summary>
    private bool invunerable = false;

    /// <summary>
    /// Indica se o player esta no ch�o
    /// </summary>
    private bool grounded = false;

    /// <summary>
    /// Inica se esta pulando
    /// </summary>
    private bool jumping = false;

    /// <summary>
    /// Indica se esta olhando para a direita. � possivel simular com o Flip X
    /// </summary>
    private bool facingRight = true;

    /// <summary>
    /// Tempo somado com atackRate para controllar a velocidade do ataque
    /// </summary>
    private float nextAttack = 0f;

    /// <summary>
    /// Sprite Renderer do player
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// Rigidbody 2D do player
    /// </summary>
    private Rigidbody2D rb2D;

    /// <summary>
    /// Animator do player
    /// </summary>
    private Animator anim;

    #endregion

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Linecast cria a linha imagin�ria do player para o ch�o
        //A linha � criada entre a posi��o do transform do player e a posi��o do GroundCheck
        //Espera tbm a camada do ch�o
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //GetButtonDown faz com que o bot�o Cima funcione somente com um clique, n�o valendo clicar e segurar
        //Se adicionar o comando de pulo em FixedUpdate o jogo fica com delay no pulo
        if (Input.GetButtonDown("Jump") && grounded)
            //Comandos de pulo
            jumping = true;


        PlayerAnimations();

        //Verifica se time � maior que next para que n�o seja possivel um monte de ataques ao mesmo tempo
        if (Input.GetButtonDown("Fire1") && grounded && Time.time > nextAttack)
            Attack();

    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(move * speed, rb2D.velocity.y);

        if ((move < 0f && facingRight) || (move > 0f && !facingRight))
        {
            Flip();
        }

        //Aplicar for�a no objeto do personagem
        if (jumping)
        {
            //Eixo X = horizontal, Eixo Y = Vertical
            //No eixo x n�o adicionar for�a para que n�o some com a for�a do pulo fazendo o player pular longe
            rb2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }

    /// <summary>
    /// Faz player trocar de lado
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Anima��es do player configuradas pelo BlendTree
    /// </summary>
    void PlayerAnimations()
    {
        //Insere o valor para VelY controlar o BlendTree
        anim.SetFloat("VelY", rb2D.velocity.y);

        //Controla o pulo e a ca�da para executar a anima��o. S� fazer a anima��o de pulo somente se 
        //n�o estiver no ch�o
        anim.SetBool("JumpFall", !grounded);

        //Indica se esta caminhando. Esta caminhando quando a velocidade � diferente de 0 e quando esta
        //no ch�o
        anim.SetBool("Walk", rb2D.velocity.x != 0f && grounded);
    }

    /// <summary>
    /// Executa o ataque do player
    /// </summary>
    void Attack()
    {
        anim.SetTrigger("Punch");

        nextAttack = Time.time + attackRate;

        //Cria instancia do Ataque
        GameObject cloneAttack = Instantiate(attackPrefab, transformSpaumAtack.position, transformSpaumAtack.rotation);

        //Se n�o tiver virado para a direita o ataque deve mudar de posi��o
        if (!facingRight)
            cloneAttack.transform.eulerAngles = new Vector3(180, 0, 180);
    }
}
