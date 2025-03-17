using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public GameObject recipePanel;
    private RecipeData recipe;
    public Image productIcon;
    IngredientSlot[] ingredientSlots;
    public Button craftBtn;



    public void Init(RecipeData recipe)
    {
        this.recipe = recipe;
        productIcon.sprite = recipe.product.stuff.Icon;

        ingredientSlots = GetComponentsInChildren<IngredientSlot>();
        for(int i = 0; i < recipe.ingredients.Count; i++)
        {
            ingredientSlots[i].Init(recipe.ingredients[i]);
        }
        craftBtn.onClick.AddListener(() => GameManager.Instance.craftingManager.ExecuteCraft(recipe));
    }
}
