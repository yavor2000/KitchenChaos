using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> _kitchenObjectSOList;

    public string recipeName;
}
}
