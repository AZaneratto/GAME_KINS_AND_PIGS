using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class Player : MonoBehaviour
{


    /// <summary>
    /// Está é a classe responsavel por toda a interação do player no game, muitos métodos e classes estão escrito em inglês pois no decorrer do projeto 
    ///achei melhor até para uma eventual postagem no GitHub.
    /// Logo abaixo são inicializados as algumas váriaveis e também o Serialize Field que ajuda muito na hora de manipular as scene e alterar valores.
    /// 
    /// Author: Alex Zaneratto
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// Logo abaixo são inicializados as algumas váriaveis e também o Serialize Field que ajuda muito na hora de manipular as scene e alterar valores.
    /// </summary>




    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayerFeet;
    AudioSource myAudioSource;


    [SerializeField] float velocidadeCorrer = 5f; //Velocidade que o Player Corre
    [SerializeField] float tamanhoPulo = 20f;      //Tamanho do Pulo
    [SerializeField] float taxaDeEscalada = 6f;    //Velocidade de Escalada nas cortinas
    [SerializeField] Vector2 hitKick = new Vector2(20f, 20f);   //Vetor direção quando o player leva um dano
    [SerializeField] Transform hurtBox;                         //  usado para desenvolver o range da arma do Player
    [SerializeField] float attackRadius = 3f;                   // Raio de ataque do Player
    [SerializeField] AudioClip jumpingSFX, attackingSFX, playerDeathSFX, getHit, walkSFX;       // Inicialização das váriaveis que usei para os sons do player

    float startingGravityScale;
    bool isHurting = false;

    // Start is called before the first frame update
    
    
    /*
     *  Aqui é a onde starta e as váriaveis criadas anteriormente são realacionadas aos componentes
     * */
    
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayerFeet = GetComponent<PolygonCollider2D>();

        startingGravityScale = myRigidBody2D.gravityScale;
        myAnimator.SetTrigger("Appering");
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        


        if (!isHurting)
        {
            Correr();
            Pular();
            Escalar();
            Attack();
            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Pig")) || myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Pig Bomb")) || myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Pig King"))) // Aqui é definido a lógica de dano do Player
            {
               PlayerHit();
            }
           

            ExitLevel();

        }
        
    }

    /*
     * Método responsável pela saída pelas portas e interação com as mesmas alterando as animações.
     * 
     */
    private void ExitLevel()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Interact")) && CrossPlatformInputManager.GetButtonDown("Vertical"))
        {

            myAnimator.SetTrigger("Dissapearing");
            
            
        }
    }

    public void TurnOffRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    /*
     * Método Responsavél por Buscar o scrip da ExitDoor e carregar a nova scene
     */
    public void LoadNextLevel()
    {
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRenderer();

    }
   

    /*
     * Responsável pela lógica de attack do Player
     */
    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1")) //Quando preciona o botão..
        {
            myAnimator.SetTrigger("Attacking");
            myAudioSource.PlayOneShot(attackingSFX);  //Audio de Ataque
            Collider2D[] enemiesToHit =  Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Pig")); //Cria a interação com as layers do Pig
            Collider2D[] enemiesToHitBomb = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Pig Bomb")); // ''''
            Collider2D[] enemiesToHitKing = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Pig King")); // ''''

            foreach (Collider2D pig in enemiesToHit)
            {
               pig.GetComponent<Pig>().EnemyHit();        // Busca no script do Pig o comportamento de Hit!

            }
            foreach (Collider2D pig in enemiesToHitBomb)
            {
                pig.GetComponent<PigBomb>().EnemyHitBomb();

            }

            foreach (Collider2D pig in enemiesToHitKing)
            {
                pig.GetComponent<PigKing>().EnemyHit();

            }





        }
    }
    /*
     * Responsável pelo comportamento quando o player leva dano
     */
    public void PlayerHit()
    {
        myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f); //Quando toma dano o player ''voa'' na direção contraria do inimigo em hitkick vezes;
        myAnimator.SetTrigger("Hitting"); //Animação de dano sofrido
        myAudioSource.PlayOneShot(getHit); //Audio do player tomando dano
        isHurting = true;
        StartCoroutine(StopHurting());  // Starta a Courotine

        FindObjectOfType<GameSession>().ProcessPlayerDeath(); // Analisa no Scrip de GameSession se o Player deve morrer.

    }

    /*
     * Uma Courotine responsavél por fazer o player ficar imovél por alguns milesimos após levar dano
     */
    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(0.5f);

        isHurting = false;

    }


    /*
     * Responsavel pela mecanica de escala nas cortinas do jogo
     */
    private void Escalar()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controleEscalando = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 velocidadeEscalada = new Vector2(myRigidBody2D.velocity.x, controleEscalando * taxaDeEscalada); //Realiza o controle de navegação quando está na cortina
            myRigidBody2D.velocity = velocidadeEscalada;

            myAnimator.SetBool("Climbing",true); //Altera a animação do player
            

            myRigidBody2D.gravityScale = 0f;  //Remove a gravidade do RigidBody do Player
            
            
        }

        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = startingGravityScale;     //Volta ao ''normal''
        }

    }

    /*
     * Método que realiza a mecanica de pulo do Player
     */
    private void Pular()
    {
        if (!myPlayerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; } //Se o Player está pisando no ''chão'' pode pular

        bool Pulando = CrossPlatformInputManager.GetButtonDown("Jump");
        
        if(Pulando)
        {
            Vector2 velocidadePulo = new Vector2(myRigidBody2D.velocity.x,tamanhoPulo); // vetor que indica o tamanho do pulo 
            myRigidBody2D.velocity = velocidadePulo;
            myAudioSource.PlayOneShot(jumpingSFX);
        }
    }


    /*
     * Responsavél pela mecanica de 'Correr'
     */
    private void Correr()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 PlayerVelocidade = new Vector2(controlThrow * velocidadeCorrer, myRigidBody2D.velocity.y);  //controle de velocidade do player
        myRigidBody2D.velocity = PlayerVelocidade;
        FlipSprite();       //Realiza o flip do personagem
        MudancaCorrendoState(); //Altera o estado

       

    }
    /*
     * Responsavél pelo som de passo ao andar no ground, porém existem detalhes a resolver e não está funcionando como devia
     */
    void stepSFX()
    {
        bool playerMovingHorizontal = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        if(playerMovingHorizontal)
        {
            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myAudioSource.PlayOneShot(walkSFX);
            }
        }
        else
        {
            myAudioSource.Stop();
        }
    }

    /*
     * Realiza a o Flip do Personagem alterando o Scale em -1;
     */
    private void FlipSprite()

    {
        bool correndoHorizontal = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if(correndoHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }

    }

    /*
     * Muda o estado para correndo no animator
     */

    private void MudancaCorrendoState()
    {
        bool correndoHorizontal = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", correndoHorizontal);



    }

    /*
     * Cria uma esfera em volta do hurtBox do player para controle de raio de dano do mesmo.
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }

    

}
