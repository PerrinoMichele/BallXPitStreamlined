using System;
using UnityEngine;


public enum BoosterType
{
    GhostBall, IronBall, VerticalLaserBall, HorizontalLaserBall, PoisonBall, VampireBall,
    LightningBall, EarthQuakeBall, BroodMotherBall, EggSackBall, DarkBall
}

[Serializable]
public class BoosterData
{
    public BoosterType BoosterType;
    public string Name;
    [TextArea] public string Description;
    public Sprite Icon;
    public int PercentageToIncrease;
}
