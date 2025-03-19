using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    //public UIManager UI;
    public Player player;
    public CraftingManager craftingManager;
    public DayNightCycle dayNightCycle;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        player = FindObjectOfType<Player>();
        craftingManager = new();
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        //UI = FindObjectOfType<UIManager>();
        //if(UI == null)
        //{
        //    UI = new GameObject("UIManager").AddComponent<UIManager>();
        //}

        //UI.InitializeUI();
    }
}
