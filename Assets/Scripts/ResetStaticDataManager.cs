using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        // Debug.LogError("ResetStaticData in MainMenu");
        BaseCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        
        ServiceLocator.Initiailze();
    }
}
