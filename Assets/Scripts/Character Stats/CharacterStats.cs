using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;
    [HideInInspector]
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    private void Awake()
    {
        characterData = Instantiate(templateData);
    }

    public int MaxHealth
    {
        get { return characterData.maxHealth; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { return characterData.currentHealth; }
        set { characterData.currentHealth = value; }
    }

    public void receiveDamage(CharacterStats attacker,CharacterStats defender)
    {
        int damage =Mathf.Max( attacker.attackData.attack - defender.attackData.defend , 0);
        defender.CurrentHealth -= damage;
        if (defender.CurrentHealth < 0)
            defender.CurrentHealth = 0;
                
    }
}
