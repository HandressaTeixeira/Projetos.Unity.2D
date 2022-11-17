using UnityEngine;

public class Player : MonoBehaviour
{
    #region Propriedades publicas
    /// <summary>
    /// Velocidade do player
    /// </summary>
    public float speed;

    /// <summary>
    /// Força do pulo
    /// </summary>
    public int jumpForce;

    /// <summary>
    /// Saúde do player
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

    #region Métodos privados

    /// <summary>
    /// Indica se esta vulneravel
    /// </summary>
    private bool invunerable = false;

    /// <summary>
    /// Indica se o player esta no chão
    /// </summary>
    private bool grounded = false;

    /// <summary>
    /// Inica se esta pulando
    /// </summary>
    private bool jumping = false;

    /// <summary>
    /// Indica se esta olhando para a direita. É possivel simular com o Flip X
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
        //Linecast cria a linha imaginária do player para o chão
        //A linha é criada entre a posição do transform do player e a posição do GroundCheck
        //Espera tbm a camada do chão
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //GetButtonDown faz com que o botão Cima funcione somente com um clique, não valendo clicar e segurar
        //Se adicionar o comando de pulo em FixedUpdate o jogo fica com delay no pulo
        if (Input.GetButtonDown("Jump") && grounded)
            //Comandos de pulo
            jumping = true;


        PlayerAnimations();

        //Verifica se time é maior que next para que não seja possivel um monte de ataques ao mesmo tempo
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

        //Aplicar força no objeto do personagem
        if (jumping)
        {
            //Eixo X = horizontal, Eixo Y = Vertical
            //No eixo x não adicionar força para que não some com a força do pulo fazendo o player pular longe
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
    /// Animações do player configuradas pelo BlendTree
    /// </summary>
    void PlayerAnimations()
    {
        //Insere o valor para VelY controlar o BlendTree
        anim.SetFloat("VelY", rb2D.velocity.y);

        //Controla o pulo e a caída para executar a animação. Só fazer a animação de pulo somente se 
        //não estiver no chão
        anim.SetBool("JumpFall", !grounded);

        //Indica se esta caminhando. Esta caminhando quando a velocidade é diferente de 0 e quando esta
        //no chão
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

        //Se não tiver virado para a direita o ataque deve mudar de posição
        if (!facingRight)
            cloneAttack.transform.eulerAngles = new Vector3(180, 0, 180);
    }
}
