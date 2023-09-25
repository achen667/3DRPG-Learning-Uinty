using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Attack Data/Data")]
public class AttackData_SO :ScriptableObject
{
    public int attack;
    public int attackRange;
    public int attackSpeed;
    public int viewRange;

    public int defend;
}
