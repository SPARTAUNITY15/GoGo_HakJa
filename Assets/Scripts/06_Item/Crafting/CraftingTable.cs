using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    public List<RecipeData> RecipesOfTable;
    public Transform content;
    public GameObject recipeSlotPref;
    public string name;

    private void Start()
    {
        RecipeSlot recipeSlot;
        for (int i = 0; i < RecipesOfTable.Count; i++)
        {
            recipeSlot = Instantiate(recipeSlotPref, content).GetComponent<RecipeSlot>();
            recipeSlot.Init(RecipesOfTable[i]);
        }
    }
}
