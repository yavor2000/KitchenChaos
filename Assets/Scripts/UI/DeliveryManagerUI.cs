using System;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
public class DeliveryManagerUI : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _deliveryManager = _serviceLocator.Get<DeliveryManager>();
        _deliveryManager.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        _deliveryManager.OnRecipeCompleted += DeliveryManager_OnRecipeComplete;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeComplete(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Debug.Log("UpdateVisual");
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in _deliveryManager.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
}
