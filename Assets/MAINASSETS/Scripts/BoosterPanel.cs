using System;
using System.Collections.Generic;
using Core.Events;
using UnityEngine;

public class BoosterPanel : PopupPanel
{
    [SerializeField] private BoosterDataList boosterDataList;
    private BoosterCard[] boosterCards;
    
    private void Start()
    {
        boosterCards = GetComponentsInChildren<BoosterCard>();
        EventBus.Subscribe<EnableBoosterPanelEvent>(EnableView);
        EventBus.Subscribe<BoosterCollectedEvent>(DisableView);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EnableBoosterPanelEvent>(EnableView);
        EventBus.Unsubscribe<BoosterCollectedEvent>(DisableView);
    }

    private void EnableView(EnableBoosterPanelEvent enableBoosterPanelEvent)
    {
        Time.timeScale = 0.1f;
        ShowPanel();

        var boosters = boosterDataList.GetRandomBoosters();
        for (int i = 0; i < boosterCards.Length; i++)
        {
            boosterCards[i].SetCardInfo(boosters[i]);
        }
    }

    private void DisableView(BoosterCollectedEvent boosterCollectedEvent)
    {
        Time.timeScale = 1.0f;
        HidePanel();
    }
}
