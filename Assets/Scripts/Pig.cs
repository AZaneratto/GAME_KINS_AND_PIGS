using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Pig : MonoBehaviour
{
    [SerializeField] float pigRunSpeed = 2f;

    Rigidbody2D pigRigidBody;
    Animator pigAnimator;

    // Start is called before the first frame update
    void Start()
    {
        pigRigidBody = GetComponent<Rigidbody2D>();
        pigAnimator = GetComponent<Animator>();

    }



        // Update is called once per frame
        void Update()
    {
        PigMoviment();
       
        
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

    

}
