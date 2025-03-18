using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptUI : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    private void Start()
    {
        HidePrompt();
    }

    //private void Update()
    //{
    //    name.text = GameManager.Instance.player.interaction.interactingObject.GetPromptName();
    //    description.text = GameManager.Instance.player.interaction.interactingObject.GetPromptDesc();
    //}

    public void ShowPrompt(IInteractable interactable)
    {
        name.enabled = true;
        description.enabled = true;
        name.text = interactable.GetPromptName();
        description.text = interactable.GetPromptDesc();
    }

    public void HidePrompt()
    {
        name.enabled = false;
        description.enabled = false;
    }
}
