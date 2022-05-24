using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
   

{
    /// <summary>
    /// Respons�vel pelo item colet�vel Diamante
    /// </summary>
    [SerializeField] int diamondValue = 100; //valor do Diamante
    [SerializeField] AudioClip diamondPickupSFX; //Som ao coletar o diamante
   
    
    /*
     * M�todo que cria o som e tamb�m adiciona o Score na Tela e destroi o gameObject do diamante no final
     */
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        AudioSource.PlayClipAtPoint(diamondPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToScore(diamondValue); //Adiciona o valor do diamante no score
        Destroy(gameObject);

    }

}
