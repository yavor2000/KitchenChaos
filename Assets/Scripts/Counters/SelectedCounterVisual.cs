using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        private ServiceLocator _serviceLocator;
        private Player _player;

        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjectArray;
    
        private void Awake()
        {
            _serviceLocator = ServiceLocator.Current;
        }

        private void Start()
        {
            _player = _serviceLocator.Get<Player>();
            Debug.Log("counter vis Start, player is " + _player);
            _player.OnSelectedCounterChanged += Player_OnSelectedCounterChange;
        }

        private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedEventArgs e)
        {
            BaseCounter bc = e.SelectedCounter;
            // Debug.Log($"{DateTime.Now} counter changed to {(bc != null ? bc.transform.position.ToString() : "null")}");
            if (e.SelectedCounter == baseCounter)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            foreach (GameObject vgo in visualGameObjectArray)
            {
                vgo.SetActive(true);
            }
        }
    
        private void Hide()
        {
            foreach (GameObject vgo in visualGameObjectArray)
            {
                vgo.SetActive(false);
            }
        }
    }
}
