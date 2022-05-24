using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PigBomb : MonoBehaviour
{
    /// <summary>
    /// Está é a classe responsavel por toda a interação do inimigo ''PIGBOMB'' no game, muitos métodos e classes estão escrito em inglês pois no decorrer do projeto 
    ///achei melhor até para uma eventual postagem no GitHub.
    /// Logo abaixo são inicializados as algumas váriaveis e também o Serialize Field que ajuda muito na hora de manipular as scene e alterar valores.
    /// </summary>
    private Transform positionPlayer; //várivel para ver onde está o player
    public float velocityEnemy = 4;  //velocidade do Pig

    Rigidbody2D pigRigidBody;
    Animator pigAnimator;
    AudioSource myAudioSource;
    BoxCollider2D myBoxCollider;
    CircleCollider2D mycircleCollider;

    [SerializeField] int pigLives = 2;   //Quantidade padrão de ''vidas'' do Pig
    [SerializeField] Vector2 hitKick = new Vector2(20f, 20f);  // Vetor de hitkick do Pig
    [SerializeField] AudioClip pigDeathSFX;  // Audio de morte do Pig
    [SerializeField] float raioView;    // Raio que o Pig passa a enxergar o Player
    [SerializeField] float rangeAttack = 10f; //Tamanho do range de ataque do Pig
    [SerializeField] float weaponSpeed = 30f;   //Velocidade que será atacado o projetil (bomba)
    [SerializeField] int bombFreq = 500;  // Frequencia de ataque de bombas

    public GameObject bombPrefab;
    public Transform pointBomb;
    public bool facingRight = false;
    bool isHurting = false;
    private int count = 0;
    private int limit = 25;

    // Start is called before the first frame update
    void Start()
    {
        pigRigidBody = GetComponent<Rigidbody2D>();
        pigAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        mycircleCollider = GetComponent<CircleCollider2D>();
        positionPlayer = GameObject.FindGameObjectWithTag("Player").transform;


    }



    // Update is called once per frame
    void Update()
    {
        if (!isHurting)
        {

            if (positionPlayer.transform.position.x < gameObject.transform.position.x && facingRight)   // Lógica responsavél em fazer o pig sempre virar na diração do Player
            { 
                Flip();
            }
            if (positionPlayer.transform.position.x > gameObject.transform.position.x && !facingRight)
            {
                Flip();
            }
            ComePlayer();
            PigAttack();
        }


    }




    void Flip()
    {
 
        facingRight = !facingRight;
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x *= -1;
        gameObject.transform.localScale = tmpScale;
    }

    /*
     * Faz com que o PigBomb siga o player enquanto ele estiver no campo de visão dele porém a principio essa mecanica está em desuso no Game
     * Visto a preferencia que o mob se mantenha parado atacando bombas.
     */
    private void ComePlayer()
    {


        if (positionPlayer.gameObject != null && Vector2.Distance(transform.position, positionPlayer.position) < (raioView))
        {

            
            transform.position = Vector2.MoveTowards(transform.position, positionPlayer.position, velocityEnemy * Time.deltaTime);
            pigAnimator.SetBool("PigRunning", true);
            if (Vector2.Distance(transform.position, positionPlayer.position) > (raioView))
            {
                pigAnimator.SetBool("PigRunning", false);

            }
        }


    }










    /*
     * Mecanica de Tomar Hit do Pig e diminuir a vida dele para causa sua morte
     */

    public void EnemyHitBomb()
    {
        if (positionPlayer.transform.position.x < gameObject.transform.position.x) //Lógica de HitKick para o Pig sempre ''voar'' na direção oposta do Player
        {
            
            pigRigidBody.velocity = hitKick * new Vector2(transform.localScale.x, 1f);
        }

        if (positionPlayer.transform.position.x > gameObject.transform.position.x )
        {
            
            pigRigidBody.velocity = hitKick * new Vector2(transform.localScale.x, 1f);
        }

        pigAnimator.SetTrigger("PigHit");
        isHurting = true;
        pigLives--; //Diminui a vida do Pig
        print(pigLives);
        StartCoroutine(StopHurting());  //Starta a Courotine responsável pelo tempo q ele fica ''imovél''
        pigLives--;
        print(pigLives);

        if (pigLives < 1) //Se vida menor que 1 entra no processo de morte
        {
            Dying();
        }

    }
    /*
     * Auxilia na mecânica de delay do PigHit
     */
    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(1f);
        isHurting = false;

    }
    /*
     * Método responsavél pela morte do PigBomb e mudança de animação
     */
    public void Dying()
    {

        pigAnimator.SetTrigger("DeadPig");//muda estado para morto
        GetComponent<CapsuleCollider2D>().enabled = false;//desativa o collider
        GetComponent<BoxCollider2D>().enabled = false; // ''
        pigRigidBody.bodyType = RigidbodyType2D.Static;// Mantém o rigidBody onde ele ''morreu''
        StartCoroutine(DestroyPig()); //Entra na Corrotine de destruição do GameObject



    }
    /*
    * Rotina de destuição do Pig após 3 segundos
    */
    IEnumerator DestroyPig()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);

    }




    /*
     * Método usado no animator para que ele faça o som de morte quando morrer  
     */

    void PigDeath()
    {
        AudioSource.PlayClipAtPoint(pigDeathSFX, Camera.main.transform.position);
    }


    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(5f);
       

    }




    /*
    * Se o player entrar no range do PigBomb ele eventualmente ataca os projeteis de tempo em tempo!
    */
    void PigAttack()
    {
        if (Vector2.Distance(transform.position, positionPlayer.position) < (rangeAttack))
        {


                if (count < limit && Time.frameCount % bombFreq == 0) // Faz a contagem de tempo para que ele ataque o projetil
                {


                    pigAnimator.SetTrigger("Attacking"); //muda o estado para atacando
                    GameObject goWeapon = (GameObject)Instantiate(bombPrefab, pointBomb.position, Quaternion.identity); //Cria o projetil que está sendo lançado, nesse caso o Prefab Bomb

                    if (facingRight) // Se olhando para a direita ele ataca para a mesma diração
                    {

                        goWeapon.GetComponent<Rigidbody2D>().AddForce(Vector2.right * weaponSpeed); //Cria o projetil e ataca na direção do player em determinada velocidade

                    }
                    else
                    {
                        

                        goWeapon.GetComponent<Rigidbody2D>().AddForce(Vector2.left * weaponSpeed);
                    }
                }
            }




        }
        
    
}
