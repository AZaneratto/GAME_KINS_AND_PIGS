using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe respons�vel pela ''ExitDoor'' que � o gameobject no qual o player troca as cenas no final de cada cen�rio e tamb�m controla as anima��es dela
/// </summary>

public class ExitDoor : MonoBehaviour
{
    [SerializeField] AudioClip openingDoorSFX, closeDoorSFX; //Variaveis para audio de abertura e fechamento das portas
    [SerializeField] float secondsToLoad = 2f;  //Segundos de carregamento para Load

    // Start is called before the first frame update

    /*
     * Controla a anima��o da abertura da porta quando o Player entra em contato com a porta
     * 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Open");
    }


    /*
      * Realiza o carregamento para a proxima tela, Fechando a porta, ativando o audio de close door e iniciando a Corrotina para a proxima tela 
      */
    public void StartLoadingNextLevel()
    {
        GetComponent<Animator>().SetTrigger("Close");
        AudioSource.PlayClipAtPoint(closeDoorSFX, Camera.main.transform.position);

        StartCoroutine(LoadNextLevel());

    }


    /*
     * Carrega a proxima cena
     */
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(secondsToLoad); //Delay para carregar a pr�xima tela(espera o 'secondLoad' segundos)

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Pega a Cena atual e armazena em currentScene...
        SceneManager.LoadScene(currentSceneIndex + 1); //Carrega a pr�xima cena ( cena atual + 1)


    }

    /*
     * Fun��o respons�vel pelo som de abertura de porta
     */
    void PlayOpeningDoorSFX()
    {
       AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position);
    }
   
}
