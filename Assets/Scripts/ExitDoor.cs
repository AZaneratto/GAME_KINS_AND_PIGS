using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] AudioClip openingDoorSFX, closeDoorSFX;
    [SerializeField] float secondsToLoad = 2f;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void StartLoadingNextLevel()
    {
        GetComponent<Animator>().SetTrigger("Close");
        AudioSource.PlayClipAtPoint(closeDoorSFX, Camera.main.transform.position);

        StartCoroutine(LoadNextLevel());

    }


    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(secondsToLoad);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);


    }

    void PlayOpeningDoorSFX()
    {
       AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position);
    }
   
}
