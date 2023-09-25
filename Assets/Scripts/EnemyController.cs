using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Singleton<EnemyController>
{
    private enum EnemyStatus{ IDLE,CHASE , ATTACK, LOSTTARGET ,DEAD}
    private EnemyStatus enemyStatus;

    public enum EnemyType { PARTROL, GUARD }
    public EnemyType enemyType;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private GameObject attackTarget;
    private NavMeshAgent agent;

    private Animator animator;


    public CharacterStats characterStats;

    public int viewRange;
    public int attackRange;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        characterStats.CurrentHealth = characterStats.MaxHealth;
    }


    void Update()
    {
        SwitchAnimation();
        ChangeEnemyStatus();
        SwitchEnemyStatus();
        
    }

    private bool PlayerNearby()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, viewRange);
        foreach(var collider in hitColliders)
        {
            if(collider.tag == "Player")
            {
                attackTarget = collider.gameObject;
                //enemyStatus =EnemyStatus.CHASE;
                return true;
            }
        }
        return false;
    }
    private void ChangeEnemyStatus()
    {
        if (!IsDead())
        {
            if (PlayerNearby())
            {
                if (Vector3.Distance(attackTarget.transform.position, transform.position) > attackRange)
                    enemyStatus = EnemyStatus.CHASE;
                else
                    enemyStatus = EnemyStatus.ATTACK;
            }
            else if (Vector3.Distance(startPosition, transform.position) <= agent.stoppingDistance)
                enemyStatus = EnemyStatus.IDLE;
            else
                enemyStatus = EnemyStatus.LOSTTARGET;
        }
        else
          enemyStatus = EnemyStatus.DEAD;

    }
    private void SwitchEnemyStatus()
    {
        switch (enemyStatus)
        {
            case EnemyStatus.IDLE:
                if(enemyType == EnemyType.GUARD)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.03f);
                }
                break;
            case EnemyStatus.LOSTTARGET:
                agent.SetDestination(startPosition);
                break;
            case EnemyStatus.CHASE:
                agent.SetDestination(attackTarget.transform.position);
                break;
            case EnemyStatus.DEAD:
                Destroy(gameObject,2f);
                break;
        }
    }

    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Attack", enemyStatus == EnemyStatus.ATTACK);
        animator.SetBool("Die", enemyStatus == EnemyStatus.DEAD);
    }

    void Hit()
    {
        var hitTargetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.receiveDamage(characterStats, hitTargetStats);
    }

    private bool IsDead()
    {
        if (characterStats.CurrentHealth == 0)  
            return true;
        else
            return false;
    }
}
