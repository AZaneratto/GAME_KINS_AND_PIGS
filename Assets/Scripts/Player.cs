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


    [SerializeField] float velocidadeCorrer = 5f;
    [SerializeField] float tamanhoPulo = 20f;
    [SerializeField] float taxaDeEscalada = 6f;

    float startingGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayerFeet = GetComponent<PolygonCollider2D>();

        startingGravityScale = myRigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Correr();
        Pular();
        Escalar();

        
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

}
