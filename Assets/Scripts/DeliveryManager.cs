using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour, IGameService
{
    [SerializeField] private RecipeListSO recipeListSO;

    private ServiceLocator _serviceLocator;
    private List<RecipeSO> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register<DeliveryManager>(this);
        _waitingRecipeSOList = new List<RecipeSO>();
        Debug.Log("DeliveryManager awake");
    }

    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;
            if (_waitingRecipeSOList.Count < _waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                _waitingRecipeSOList.Add(waitingRecipeSO);
                Debug.Log(waitingRecipeSO.recipeName);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeSOList[i];

            if (waitingRecipeSO._kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO koso in waitingRecipeSO._kitchenObjectSOList)
                {
                    // Cycle through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO pkso in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycle through all ingredients in the Plate
                        if (pkso == koso)
                        {
                            // Ingredients matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    Debug.Log("Player delivered the correct recipe!");
                    _waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        // Player did not deliver a correct recipe
        Debug.Log("Player did not deliver a correct recipe");
    }
}
