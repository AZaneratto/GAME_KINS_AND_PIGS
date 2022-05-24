using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    /// <summary>
    /// Classe que controla o GameObject da Bomba
    /// 
    /// </summary>

    [SerializeField] float radius = 3f;  //Raio de dano da bomba
    [SerializeField] Vector2 explosionForce = new Vector2(200f, 100f); // Força da explosão
    [SerializeField] AudioClip explodeSFX, burningSFX; //audio de explosão e queima da bomba

    Animator myAnimator;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    /*
     *Responsável pela mecanica de explosão da bomba
     */
    void ExplodeBomb()
    {
        
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player")); //Ativa a colisão com o Player
        myAudioSource.PlayOneShot(explodeSFX); //som de explosão

        if (playerCollider) //Se colidir
        {
            playerCollider.GetComponent<Rigidbody2D>().AddForce(explosionForce); //Adiciona Força ao RigidBody do Player para q ele ''voe longe''
            playerCollider.GetComponent<Player>().PlayerHit(); // Faz com que o Player tome dano
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myAnimator.SetTrigger("Burn"); //inicia a animação de queimada
        myAudioSource.PlayOneShot(burningSFX); //som de bomba queimando
    }

    void DestroyBomb()
    {
        
        Destroy(gameObject); //destroi a bomba
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius); //auxilia na criação do raio de explosão da bomba
    }
}
