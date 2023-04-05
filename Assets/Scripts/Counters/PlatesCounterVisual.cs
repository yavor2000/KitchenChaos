using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Counters
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform counterTopPoint;
        [SerializeField] private Transform plateVisualPrefab;

        private List<GameObject> _plateVisualGameObjectList;
        private const float PlateSizeY = 0.1f;

        private void Awake()
        {
            _plateVisualGameObjectList = new List<GameObject>();
        }

        private void Start()
        {
            platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
            platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
        }

        private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
        {
            int plateVisualGameObjectListCount = _plateVisualGameObjectList.Count;
            GameObject go = _plateVisualGameObjectList.ElementAt(plateVisualGameObjectListCount - 1);
            _plateVisualGameObjectList.RemoveAt(plateVisualGameObjectListCount - 1);
            Destroy(go);
        }

        private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
        {
            Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
            // Vector3 newPos = plateVisualTransform.position;
            // newPos.y += _platesCount * 0.2f;
            // plateVisualTransform.position = newPos;
            plateVisualTransform.localPosition = new Vector3(0, _plateVisualGameObjectList.Count * PlateSizeY, 0);
            _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        }
    }
}
