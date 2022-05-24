using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    /// <summary>
    /// Respons�vel pelo item colet�vel Cora��o
    /// </summary>
    // Start is called before the first frame update

    [SerializeField] int heartValue = 1;   //valor de cada gameObject de cora��o
    [SerializeField] AudioClip heartPickupSFX;  //som ao pegar o cora��o


    /*
     * M�todo que cria o som e tamb�m adiciona o Score na Tela e destroi o gameObject do diamante no final
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {

        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToLives(heartValue); //Adiciona o valor do cora��o na live bar
        Destroy(gameObject);
    }
}
