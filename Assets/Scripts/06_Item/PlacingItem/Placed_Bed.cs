using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_Bed : Placed_Item
{
    public override void SubscribeMethod()
    {
        Sleep();
    }

    private void Sleep()
    {
        GameManager.Instance.player.condition.Heal(100);

        StartCoroutine(GameManager.Instance.dayNightCycle.Sleeping());
    }
}
