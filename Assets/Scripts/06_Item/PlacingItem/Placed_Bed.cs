using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_Bed : MonoBehaviour, IInteractable
{
    public void GetPromptInfo()
    {
        
    }

    public void SubscribeMethod()
    {
        Sleep();
    }

    private void Sleep()
    {
        GameManager.Instance.player.condition.Heal(100);

            // ³·¹ã ¹Ù²î°Ô 
    }
}
