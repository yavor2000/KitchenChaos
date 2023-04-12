using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class PlateIconsSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectSO koso)
    {
        image.sprite = koso.sprite;
    }
}
}
