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
    [SerializeField] Vector2 explosionForce = new Vector2(200f, 100f); // For�a da explos�o
    [SerializeField] AudioClip explodeSFX, burningSFX; //audio de explos�o e queima da bomba

    Animator myAnimator;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    /*
     *Respons�vel pela mecanica de explos�o da bomba
     */
    void ExplodeBomb()
    {
        
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player")); //Ativa a colis�o com o Player
        myAudioSource.PlayOneShot(explodeSFX); //som de explos�o

        if (playerCollider) //Se colidir
        {
            playerCollider.GetComponent<Rigidbody2D>().AddForce(explosionForce); //Adiciona For�a ao RigidBody do Player para q ele ''voe longe''
            playerCollider.GetComponent<Player>().PlayerHit(); // Faz com que o Player tome dano
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myAnimator.SetTrigger("Burn"); //inicia a anima��o de queimada
        myAudioSource.PlayOneShot(burningSFX); //som de bomba queimando
    }

    void DestroyBomb()
    {
        
        Destroy(gameObject); //destroi a bomba
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius); //auxilia na cria��o do raio de explos�o da bomba
    }
}
