using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class itemWithCount
{
    public ItemData stuff;
    public int stuff_count;
}

[CreateAssetMenu(fileName = "Recipe", menuName = "New Recipe")]
public class RecipeData : ScriptableObject
{
    public List<itemWithCount> ingredients = new();
    public itemWithCount product = new();


}
