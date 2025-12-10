using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;

    public Button startButton;  // Botão "Jogar"
    public Button restartButton; // Novo botão "Reiniciar"
    public Image startImage;
    public TMP_Text startTextTMP;
    public TMP_Text YouLose;
    public TMP_Text YouWin;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame); // Ativa o botão de reinício

        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false); // Só aparece depois
        YouLose.gameObject.SetActive(false);
        YouWin.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        isGameActive = true;

        startButton.gameObject.SetActive(false);
        startImage.gameObject.SetActive(false);
        startTextTMP.gameObject.SetActive(false);
        YouLose.gameObject.SetActive(false);
        YouWin.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        Debug.Log("Jogo iniciado!");
    }

    public void ShowGameOver(bool won)
    {
        isGameActive = false;

        if (won)
        {
            YouWin.gameObject.SetActive(true);
        }
        else
        {
            YouLose.gameObject.SetActive(true);
        }

        restartButton.gameObject.SetActive(true); // Ativa o botão de reinício
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (!isGameActive && startButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (!isGameActive && restartButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
        }
    }

}
