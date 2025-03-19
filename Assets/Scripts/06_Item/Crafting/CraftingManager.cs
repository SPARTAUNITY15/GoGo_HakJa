using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingManager
{

    // 재료 확인 후 제작 허가

    private bool CheckCraftAvailable(RecipeData recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!(Inventory.Instance.itemCounts.ContainsKey(ingredient.stuff)) || (Inventory.Instance.itemCounts[ingredient.stuff] < ingredient.stuff_count))
            {
                Debug.Log("재료부족");
                return false;
            }
        }
        return true;
    }

    public void ExecuteCraft(RecipeData recipe)
    {
        if (!CheckCraftAvailable(recipe)) return;

        foreach (var ingredient in recipe.ingredients)
        {
            for (int i = 0; i < ingredient.stuff_count; i++)
            {
                Inventory.Instance.RemoveItem(ingredient.stuff);
            }
        }

        for (int i = 0; i < recipe.product.stuff_count; i++)
        {
            Inventory.Instance.AddItem(recipe.product.stuff);
        }

        UIManager.Instance.inventoryUI.UpdateUI();
    }
}
