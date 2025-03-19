using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void SubscribeMethod();

    public string GetPromptName();
    public string GetPromptDesc();

}
public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask InteractableLayerMask;
    public IInteractable interactingObject;
    //public PromptUI promptUI;

    private void Start()
    {
        //promptUI = GameManager.Instance.UI.promptUI;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, InteractableLayerMask))
        {
            interactingObject = hit.transform.gameObject.GetComponentInParent<IInteractable>();

            if(interactingObject == null)
            {
                Debug.LogError($"{hit.transform.gameObject} ������Ʈ�� InteractableLayerMask �ӿ��� IInteractable ������Ʈ�� �����ϴ�.");
            }

            UIManager.Instance.promptUI.ShowPrompt(interactingObject);
        }
        else
        {
            // ����
            interactingObject = null;
            UIManager.Instance.promptUI.HidePrompt();
        }
    }
}
