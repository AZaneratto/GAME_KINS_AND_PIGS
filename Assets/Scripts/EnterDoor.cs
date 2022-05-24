using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] AudioClip openingDoorSFX, closeDoorSFX; //Sons de abertura e fechamento da porta de entrada

    void Start()
    {
        GetComponent<Animator>().SetTrigger("Open"); //anima a abertura da porta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position); //ativa o som de abertura
    }

    void PlayCloseDoorSFX()
    {
        AudioSource.PlayClipAtPoint(closeDoorSFX, Camera.main.transform.position); //ativa o som de fechamento da porta
    }

}
