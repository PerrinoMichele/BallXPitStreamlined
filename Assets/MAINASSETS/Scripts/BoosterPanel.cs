using System;
using System.Collections.Generic;
using Core.Events;
using UnityEngine;

public class BoosterPanel : PopupPanel
{
    [SerializeField] private BoosterDataList boosterDataList;
    private BoosterCard[] boosterCards;

    // Salviamo valori precedenti per ripristinarli correttamente
    private float previousTimeScale = 1f;
    private float previousFixedDeltaTime = 0.02f;
    private bool previousAudioPause = false;

    // Riferimenti per disabilitare/riabilitare input del player
    private DragMoveAmplified playerMovement;
    private List<Joystick> disabledJoysticks;
    
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
        // Memorizza lo stato corrente e metti il gioco in pausa completa
        previousTimeScale = Time.timeScale;
        previousFixedDeltaTime = Time.fixedDeltaTime;
        previousAudioPause = AudioListener.pause;

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        AudioListener.pause = true;

        // Disabilita i componenti di movimento/input del player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<DragMoveAmplified>();
            if (playerMovement != null)
                playerMovement.enabled = false;
        }

        // Disabilita le joystick UI attive e memorizza quelli disabilitati per riabilitarli dopo
        disabledJoysticks = new List<Joystick>();
        var joysticks = FindObjectsOfType<Joystick>(true);
        foreach (var js in joysticks)
        {
            if (js.enabled)
            {
                disabledJoysticks.Add(js);
                js.enabled = false;
            }
        }

        ShowPanel();

        var boosters = boosterDataList.GetRandomBoosters();
        for (int i = 0; i < boosterCards.Length; i++)
        {
            boosterCards[i].SetCardInfo(boosters[i]);
        }
    }

    private void DisableView(BoosterCollectedEvent boosterCollectedEvent)
    {
        // Ripristina lo stato precedente
        Time.timeScale = previousTimeScale;
        Time.fixedDeltaTime = previousFixedDeltaTime;
        AudioListener.pause = previousAudioPause;

        // Riabilita movimento del player se lo avevamo disabilitato
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement = null;
        }

        // Riabilita le joystick che abbiamo disabilitato
        if (disabledJoysticks != null)
        {
            foreach (var js in disabledJoysticks)
            {
                if (js != null) js.enabled = true;
            }
            disabledJoysticks = null;
        }

        HidePanel();
    }
}
