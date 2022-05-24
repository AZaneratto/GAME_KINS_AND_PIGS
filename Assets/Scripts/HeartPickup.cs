using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    /// <summary>
    /// Responsável pelo item coletável Coração
    /// </summary>
    // Start is called before the first frame update

    [SerializeField] int heartValue = 1;   //valor de cada gameObject de coração
    [SerializeField] AudioClip heartPickupSFX;  //som ao pegar o coração


    /*
     * Método que cria o som e também adiciona o Score na Tela e destroi o gameObject do diamante no final
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {

        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToLives(heartValue); //Adiciona o valor do coração na live bar
        Destroy(gameObject);
    }
}
