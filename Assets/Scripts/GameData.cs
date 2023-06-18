using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public Color[] stairColors;
    public float totalMoney;
    public float incomeAmount;
    public float unitsAmount;
    public int unitsLevel;
    public int incomeLevel;
    public int levelIndex;

    [Button]
    void ResetData()
    {
        totalMoney = 250f;
        incomeAmount = 50f;
        unitsAmount = 50f;
        unitsLevel = 1;
        incomeLevel = 1;
        levelIndex = 1;
    }
}
