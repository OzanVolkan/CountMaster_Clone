using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public Color[] stairColors;
    public float totalMoney;
    public float incomeAmount;
    public float unitsAmount;
    public int unitsLevel;
    public int incomeLevel;
}
