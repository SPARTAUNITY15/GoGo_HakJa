using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public StatManager statManager;
    public InteractionHandler interaction;
    public ItemPlaceController itemPlaceController;
    public PlayerEquip playerEquip;
    public AudioClip _MainScene;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        statManager = GetComponent<StatManager>();
        interaction = GetComponent<InteractionHandler>();
        itemPlaceController = GetComponent<ItemPlaceController>();
        playerEquip = GetComponent<PlayerEquip>();
    }

    public void Start()
    {
        AudioManager.Instance.PlayBGM(_MainScene);
    }
}
