using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayerStateData", menuName = "Data/State Data/Look For Player State")]

public class DataLookForPlayerState : ScriptableObject
{
    public float timeInTurns = 0.5f;
    public int amountOfTurns = 2;
}
