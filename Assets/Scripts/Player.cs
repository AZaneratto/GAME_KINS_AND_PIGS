using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class Player : MonoBehaviour
{
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayerFeet;
    AudioSource myAudioSource;


    [SerializeField] float velocidadeCorrer = 5f;
    [SerializeField] float tamanhoPulo = 20f;
    [SerializeField] float taxaDeEscalada = 6f;
    [SerializeField] Vector2 hitKick = new Vector2(20f, 20f);
    [SerializeField] Transform hurtBox;
    [SerializeField] float attackRadius = 3f;
    [SerializeField] AudioClip jumpingSFX, attackingSFX, playerDeathSFX, getHit, walkSFX;

    float startingGravityScale;
    bool isHurting = false;

    // Start is called before the first frame update
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
            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Pig")))
            {
               // PlayerHit();
            }
           

            ExitLevel();

        }
        
    }


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

    public void LoadNextLevel()
    {
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRenderer();

    }
   


    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            myAnimator.SetTrigger("Attacking");
            myAudioSource.PlayOneShot(attackingSFX);
            Collider2D[] enemiesToHit =  Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Pig"));

            foreach(Collider2D pig in enemiesToHit)
            {
                pig.GetComponent<Pig>().EnemyHit();

            }


        }
    }

    public void PlayerHit()
    {
        myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);
        myAnimator.SetTrigger("Hitting");
        myAudioSource.PlayOneShot(getHit);
        isHurting = true;
        StartCoroutine(StopHurting());

        FindObjectOfType<GameSession>().ProcessPlayerDeath();

    }

    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(1f);

        isHurting = false;

    }

    private void Escalar()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controleEscalando = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 velocidadeEscalada = new Vector2(myRigidBody2D.velocity.x, controleEscalando * taxaDeEscalada);
            myRigidBody2D.velocity = velocidadeEscalada;

            myAnimator.SetBool("Climbing",true);
            

            myRigidBody2D.gravityScale = 0f;
            
            
        }

        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = startingGravityScale;
        }

    }
    private void Pular()
    {
        if (!myPlayerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        bool Pulando = CrossPlatformInputManager.GetButtonDown("Jump");
        
        if(Pulando)
        {
            Vector2 velocidadePulo = new Vector2(myRigidBody2D.velocity.x,tamanhoPulo);
            myRigidBody2D.velocity = velocidadePulo;
            myAudioSource.PlayOneShot(jumpingSFX);
        }
    }

    private void Correr()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 PlayerVelocidade = new Vector2(controlThrow * velocidadeCorrer, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = PlayerVelocidade;
        FlipSprite();
        MudancaCorrendoState();

       

    }

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


    private void FlipSprite()

    {
        bool correndoHorizontal = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if(correndoHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }

    }

    private void MudancaCorrendoState()
    {
        bool correndoHorizontal = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", correndoHorizontal);
        


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }

    

}
