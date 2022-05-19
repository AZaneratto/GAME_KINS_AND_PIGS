using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int heartValue = 1;
    [SerializeField] AudioClip heartPickupSFX; 

    private void OnTriggerEnter2D(Collider2D collision)
    {

        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToLives(heartValue);
        Destroy(gameObject);
    }
}
