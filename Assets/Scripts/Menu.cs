using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe que controla as LoadScene do Jogo e o fechamento dele
/// </summary>
public class Menu : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); //Entra no Menu
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(6); //Cena de créditos
    }

    /*
     * Método usado no botão para fechar o game
     */
    public void CloseGame()
    {

    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();

    }
}