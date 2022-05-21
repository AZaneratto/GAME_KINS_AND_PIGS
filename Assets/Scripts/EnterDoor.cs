using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] AudioClip openingDoorSFX, closeDoorSFX;

    void Start()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position);
    }

    void PlayCloseDoorSFX()
    {
        AudioSource.PlayClipAtPoint(closeDoorSFX, Camera.main.transform.position);
    }

}
