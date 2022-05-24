using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    /// <summary>
    /// Aqui são controlados diversas partes do game, como o controle de vidas do player, score de diamantes.
    /// Processo de morte do Player, adição de coração e etc 
    /// </summary>
    [SerializeField] int playerLives = 3, score = 0;
    [SerializeField] Text scoreText, livesText;
    [SerializeField] Image[] hearts;

    
    /*
     * Verifica o tamannho de Game Sessions
     */
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    private void Start()
    {
        livesText.text = playerLives.ToString(); //pega a quantidade de coração e transforma em uma String para usar na UI
        scoreText.text = score.ToString(); // pega o score e transforma numa String...
    }

    public void AddToScore(int value)
    {
        score += value; // adiciona o valor do Score
        scoreText.text = score.ToString(); //Transforma o valor adiciona em String para a UI
    }

    /*
     * Faz a coleta e update de coração conforme o player sofre dano e pega corações;
     */
    public void AddToLives(int value)
    {


        playerLives++;
        if(playerLives >= 3)
        {
            playerLives = 3;
        }
        UpdateHearts();
        livesText.text = playerLives.ToString();

    }

    
    /*
     * Processo de morte do Player
     */
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame(); //Reseta o game
        }
    }
    /*
     * Quando toma dano, diminui os corações, atualiza o UI
     */
    private void TakeLife()
    {
        playerLives--;
        UpdateHearts();
        livesText.text = playerLives.ToString();
    }
    /*
     * Destroy o game Object a fim de resetar o Game carregando a cena de ''derrota''
     */
    private void ResetGame()
    {
        SceneManager.LoadScene(5);
        Destroy(gameObject);
    }
    /*
     * Cena de créditos do game
     */
    public void CreditsGame()
    {
        SceneManager.LoadScene(6);
    }

    /*
     * Atualiza os corações (imagens dele no UI) para que possa ser usado para o Live Bar
     */
    void UpdateHearts()
    {
        for( int i =0; i<hearts.Length; i++)
        {
            if( i <playerLives)
                {
                    hearts[i].enabled = true;
                }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

}
