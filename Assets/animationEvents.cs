using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEvents : MonoBehaviour
{
    private PlayerCombatController pcc;
    void Start()
    {
        pcc = GetComponentInParent<PlayerCombatController>();
    }

    public void CheckAttackHitbox()
    {
        pcc.CheckAttackHitBox();
    }

    public void FinishAttack()
    {
        pcc.FinishAttack();
    }
}
