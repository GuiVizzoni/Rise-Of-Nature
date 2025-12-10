using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCDialogo : MonoBehaviour
{
    public NPCConversation myConversation;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) // Verifica se o botão do mouse foi pressionado
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
        
    }
}
