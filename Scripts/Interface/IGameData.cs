using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameData
{
    public void LoadData(int balacce, int level, List<int> cardNumberOfEachLevel, List<int> timeOfEachLevel);
    public void SaveData(ref int balacce, ref int level);
}
