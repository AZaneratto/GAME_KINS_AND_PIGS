using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Pig : MonoBehaviour
{
    [SerializeField] float pigRunSpeed = 2f;

    Rigidbody2D pigRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        pigRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipSprite();
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
