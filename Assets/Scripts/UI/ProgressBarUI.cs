using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress _hasProgress;
    
    private void Start()
    {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress == null)
        {
            Debug.LogError($"GameObject {hasProgressGameObject} does not have a component that implements IHasProgress!");
        }
        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float barImageFillAmount = e.ProgressNormalized;
        barImage.fillAmount = barImageFillAmount;
        if (barImageFillAmount == 0f || barImageFillAmount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
}
