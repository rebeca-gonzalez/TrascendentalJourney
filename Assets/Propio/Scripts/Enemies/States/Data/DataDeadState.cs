using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State")]

public class DataDeadState : ScriptableObject
{
    public GameObject deathChunkParticles;
    public GameObject deathBloodParticles;
}
