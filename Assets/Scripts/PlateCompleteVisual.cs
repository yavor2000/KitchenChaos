using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;
    
    private void Start()
    {
        HideAllIngredients();
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        ShowIngredient(e.KitchenObjectSO);
    }

    private void HideAllIngredients()
    {
        foreach (KitchenObjectSO_GameObject kogo in kitchenObjectSOGameObjectList)
        {
            kogo.gameObject.SetActive(false);
        }
    }
    
    private void ShowIngredient(KitchenObjectSO koso)
    {
        foreach (KitchenObjectSO_GameObject kogo in kitchenObjectSOGameObjectList)
        {
            if (koso == kogo.KitchenObjectSO) {
                kogo.gameObject.SetActive(true);
            }
        }
    }
}
