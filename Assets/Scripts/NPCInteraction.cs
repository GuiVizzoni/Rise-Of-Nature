using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractionTMP : MonoBehaviour
{
    public GameObject dialogueUI; // painel da fala
    public TextMeshProUGUI dialogueText; // componente TMP de texto
    public string message = "Olá, viajante! Seja bem-vindo!";
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDialogue();
        }
    }

    void ToggleDialogue()
    {
        if (dialogueUI.activeSelf)
        {
            dialogueUI.SetActive(false);
        }
        else
        {
            dialogueText.text = message;
            dialogueUI.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            dialogueUI.SetActive(false);
        }
    }
}
