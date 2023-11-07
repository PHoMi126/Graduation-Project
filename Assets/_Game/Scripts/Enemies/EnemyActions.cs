using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour, IDamagable
{
    [Header("NavMesh")]
    public NavMeshAgent agent;
    [SerializeField] float range; //radius of sphere
    [SerializeField] Transform centrePoint; //centre of the area the agent wants to move around in
                                            //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    [Header("Animations")]
    public AnimStates animStates;
    [SerializeField] float waitTimer = 0f;

    [Header("GameObjects")]
    [SerializeField] GameObject zombie;
    [SerializeField] PlayerDetection playerDetect;
    [SerializeField] GameObject attackArea;

    [Header("Enemy Stats")]
    [SerializeField] Transform player;
    public float attackRange;
    public float attackTimer;
    private float enemyHP = 100f;

    private bool isAttacking = false;
    private bool isOwnDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        DeActiveAttack();
    }

    void Update()
    {
        if (playerDetect.FindTheTarget() != null)
        {
            PlayerDetected();
        }
        else
        {
            Wander();
        }

        ZombieState();

        PlayerInAttackRange();
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void ZombieState()
    {
        agent.speed = 1.0f;
        waitTimer += Time.deltaTime;
        if (animStates != null)
        {
            if (animStates.isDead)
            {
                TakeDamage(enemyHP);
                agent.SetDestination(agent.transform.position);
            }
        }
    }

    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            //animStates.ResetAnim();
            animStates.ChangeAnim(AnimStates.AnimState.zIdle);

            if (RandomPoint(centrePoint.position, range, out Vector3 point) && waitTimer >= 4f) //pass in our centre point and radius of area
            {
                //animStates.ResetAnim();
                animStates.ChangeAnim(AnimStates.AnimState.zWalk);
                waitTimer = 0f;
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }
    }

    void PlayerDetected()
    {
        if (playerDetect.FindTheTarget() != null)
        {
            zombie.transform.LookAt(playerDetect.FindTheTarget().transform);
            RunToPlayer();
        }
    }

    void RunToPlayer()
    {
        agent.speed = 2.0f;
        agent.SetDestination(playerDetect.player.transform.position);
        //animStates.ResetAnim();
        animStates.ChangeAnim(AnimStates.AnimState.zRun);
    }

    void PlayerInAttackRange()
    {
        if (player != null)
        {
            if (Vector3.Distance(player.position, transform.position) <= attackRange)
            {
                if (isOwnDead == true)
                    return;

                attackTimer += Time.deltaTime;

                if (attackTimer > 0.5f)
                {
                    ActiveAttack();
                }

                if (attackTimer > 0.75f)
                {
                    DeActiveAttack();
                    attackTimer = 0f;
                }
            }
            else if (isOwnDead == false)
            {
                PlayerDetected();
                agent.isStopped = false;
            }
        }

    }

    private void ActiveAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animStates.ResetAnim();
            animStates.ChangeAnim(AnimStates.AnimState.zAttack);
            attackArea.SetActive(true);
            agent.isStopped = true;
            Debug.Log("Att");
        }
    }

    private void DeActiveAttack()
    {
        isAttacking = false;
        attackArea.SetActive(false);
        agent.isStopped = false;
    }

    void Dying()
    {
        if (!isOwnDead)
        {
            isOwnDead = true;
            //animStates.ResetAnim();
            animStates.ChangeAnim(AnimStates.AnimState.zDead);
            animStates.isDead = true;
            attackArea.SetActive(false);
            Invoke(nameof(RemoveCorpse), 3f);
        }
    }

    void RemoveCorpse()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        if (enemyHP > 0f)
        {
            enemyHP -= dmg;
            if (enemyHP <= 0f)
                Dying();
        }

    }
}
