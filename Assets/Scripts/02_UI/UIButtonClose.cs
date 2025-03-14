using UnityEngine;
using UnityEngine.UI;

public class UIButtonClose : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.HideCurrentUI());
    }
}
