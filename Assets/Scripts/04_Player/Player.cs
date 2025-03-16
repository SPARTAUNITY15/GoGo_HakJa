using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public StatManager statManager;
    public InteractionHandler interaction;
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        statManager = GetComponent<StatManager>();
        interaction = GetComponent<InteractionHandler>();    
    }
}
