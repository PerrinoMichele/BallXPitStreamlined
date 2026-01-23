using System;
using UnityEngine;


public enum BoosterType { PlayerSpeed, BallSpeed, BallDamage }
[Serializable]
public class BoosterData
{
    public BoosterType BoosterType;
    public string Name;
    [TextArea] public string Description;
    public Sprite Icon;
    public int PercentageToIncrease;
}
