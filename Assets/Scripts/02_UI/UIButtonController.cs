using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public string uiName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.ShowUI(uiName));
    }
}
