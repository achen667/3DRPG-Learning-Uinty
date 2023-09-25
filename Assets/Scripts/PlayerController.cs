using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject attackTarget;
    private Animator animator;
    public int attackRange;
    private CharacterStats characterStats;

    void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        characterStats.CurrentHealth =  characterStats.MaxHealth;
        
    }
    void Start()
    {
        MouseManager.Instance.OnMapClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += Attack;
        if (GameManager.Instance == null)
            Debug.Log("error0");
        GameManager.Instance.RigisterPlayer(characterStats);

        DontDestroyOnLoad(this);
    }
    void Update()
    {
        SwitchAnimation();
    }


    void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        animator.SetBool("Attack", false);
        agent.SetDestination(target);
    }
    void Attack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToEnemy());
        }
    }

    IEnumerator MoveToEnemy()
    {
        
         
        transform.LookAt(attackTarget.transform.position);
        while (attackTarget != null && Vector3.Distance(transform.position,attackTarget.transform.position) > attackRange)
        {
            transform.LookAt(attackTarget.transform.position);
            agent.SetDestination(attackTarget.transform.position);
            yield return null;
        }
        //Debug.Log("attack!");
        animator.SetBool("Attack", true);
        
    }

    void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    void Hit()
    {
        var hitTargetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.receiveDamage(characterStats, hitTargetStats);
    }
}
