using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterDataList", menuName = "Data/BoosterDataList")]
public class BoosterDataList : ScriptableObject
{
    [SerializeField] private BoosterData[] boosterDataList;


    public List<BoosterData> GetRandomBoosters()
    {
        return boosterDataList.OrderBy(_ => Random.value).Take(3)
            .ToList();
    }
}
