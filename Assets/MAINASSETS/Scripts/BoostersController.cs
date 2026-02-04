using System;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using UnityEngine;

public class BoostersController : MonoBehaviour
{
    public static BoostersController Instance;

    private Dictionary<BoosterData, int> activeBoosterData;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        activeBoosterData = new Dictionary<BoosterData, int>();
        EventBus.Subscribe<BoosterCollectedEvent>(AddBooster);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BoosterCollectedEvent>(AddBooster);
    }

    public float GetBoosterImplementedValue(BoosterType boosterType, float defaultValue)
    {
        var matching = activeBoosterData
            .Where(kvp => kvp.Key != null && kvp.Key.BoosterType == boosterType)
            .ToList();

        var stacks = matching.Sum(kvp => kvp.Value);
        if (stacks == 0) return defaultValue;

        float percentPerStack = matching[0].Key.PercentageToIncrease;

        float increase = defaultValue * (percentPerStack / 100f) * stacks;
        return defaultValue + increase;

    }

    private void AddBooster(BoosterCollectedEvent boosterCollectedEvent)
    {
        var boosterData = boosterCollectedEvent.BoosterData;
        if (!activeBoosterData.TryAdd(boosterData, 1))
        {
            activeBoosterData[boosterData] += 1;
        }

        switch (boosterData.BoosterType)
        {
            case BoosterType.GhostBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Ghost));
                break;
            case BoosterType.IronBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Iron));
                break;
            case BoosterType.VerticalLaserBall:
                EventBus.Publish(new BallCollectedEvent(BallType.VerticalLaser));
                break;
            case BoosterType.HorizontalLaserBall:
                EventBus.Publish(new BallCollectedEvent(BallType.HorizontalLaser));
                break;
            case BoosterType.PoisonBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Poison));
                break;
            case BoosterType.VampireBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Vampire));
                break;
            case BoosterType.LightningBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Lightning));
                break;
            case BoosterType.EarthQuakeBall:
                EventBus.Publish(new BallCollectedEvent(BallType.EarthQuake));
                break;
            case BoosterType.BroodMotherBall:
                EventBus.Publish(new BallCollectedEvent(BallType.BroodMother));
                break;
            case BoosterType.EggSackBall:
                EventBus.Publish(new BallCollectedEvent(BallType.EggSack));
                break;
            case BoosterType.DarkBall:
                EventBus.Publish(new BallCollectedEvent(BallType.Dark));
                break;
            default:
                break;
        }
    }
}
