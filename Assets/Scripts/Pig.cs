using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Pig : MonoBehaviour
{



    private Transform positionPlayer;
    public float velocityEnemy=4;

    Rigidbody2D pigRigidBody;
    Animator pigAnimator;
    AudioSource myAudioSource;
    BoxCollider2D myBoxCollider;
    CircleCollider2D mycircleCollider;

    [SerializeField] int pigLives = 20;
    [SerializeField] float pigRunSpeed = 2f;
    [SerializeField] Vector2 hitKick = new Vector2(20f, 20f);
    [SerializeField] AudioClip pigDeathSFX;
    

    bool isHurting = false;
    bool left;
    // Start is called before the first frame update
    void Start()
    {
        pigRigidBody = GetComponent<Rigidbody2D>();
        pigAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        mycircleCollider = GetComponent<CircleCollider2D>();
        positionPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        

    }



        // Update is called once per frame
        void Update()
    {
        if(!isHurting)
        {
            viewPlayer();
        }
            

    }

   

    private void ComePlayer()
    {

        if(positionPlayer.gameObject != null)
        {
            pigAnimator.SetTrigger("Running");
            transform.position = Vector2.MoveTowards(transform.position, positionPlayer.position, velocityEnemy * Time.deltaTime);
        }
        

    }

    private void viewPlayer()
    {

        pigAnimator.SetTrigger("idle");
        if (mycircleCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            ComePlayer();
        }
    }


    




    public void EnemyHit()
    {

        if (left)
        {
            pigRigidBody.velocity = hitKick * new Vector2(transform.localScale.x, 1f);
        }
        else
        {
            pigRigidBody.velocity = hitKick * new Vector2(transform.localScale.x, 1f);
        }

        
        pigAnimator.SetTrigger("PigHit");
        isHurting = true;
        pigLives--;
        print(pigLives);
        StartCoroutine(StopHurting());
        pigLives--;
        print(pigLives);
        pigAnimator.SetTrigger("PigRunning");
        if (pigLives < 1)
        {
            Dying();
        }

    }

    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(1f);
        isHurting = false;

    }

    public void Dying()
    {

        pigAnimator.SetTrigger("DeadPig");
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        pigRigidBody.bodyType = RigidbodyType2D.Static;
        StartCoroutine(DestroyPig());


    }

    IEnumerator DestroyPig()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipSprite();
    }

    private void PigMoviment()
    {
        if (IsFacingLeft())
        {
            pigRigidBody.velocity = new Vector2(-pigRunSpeed, 0f);
            
        }
        else
        {
            pigRigidBody.velocity = new Vector2(pigRunSpeed, 0f);
            
        }
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(pigRigidBody.velocity.x), 1f);

    }

    private bool IsFacingLeft()
    {
        return transform.localScale.x > 0;
            
    }

    void PigDeath()
    {
        AudioSource.PlayClipAtPoint(pigDeathSFX, Camera.main.transform.position);
    }


}
