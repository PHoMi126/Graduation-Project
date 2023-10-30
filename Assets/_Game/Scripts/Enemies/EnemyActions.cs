using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour, IDamagable
{
    [Header("NavMesh")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float range; //radius of sphere
    [SerializeField] Transform centrePoint; //centre of the area the agent wants to move around in
                                            //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    [Header("Animations")]
    [SerializeField] AnimStates animStates;
    [SerializeField] float waitTimer = 0f;

    [Header("GameObjects")]
    [SerializeField] GameObject zombie;
    [SerializeField] PlayerDetection playerDetect;

    [Header("Enemy Stats")]
    private float enemyHP = 100f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerDetect.FindTheTarget() != null)
        {
            PlayerDetected();
        }
        else
        {
            ZombieState();
        }
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
            if (animStates.animator.GetCurrentAnimatorStateInfo(0).IsName("zDead"))
            {
                TakeDamage(enemyHP);
                agent.isStopped = true;
            }
            else
            {
                Wander();
            }
        }
    }

    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            animStates.ChangeAnim(AnimStates.AnimState.zIdle);

            if (RandomPoint(centrePoint.position, range, out Vector3 point) && waitTimer >= 4f) //pass in our centre point and radius of area
            {
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
        GetComponent<NavMeshAgent>().SetDestination(playerDetect.player.transform.position);
        animStates.ChangeAnim(AnimStates.AnimState.zRun);

    }

    void Attack()
    {
        animStates.ChangeAnim(AnimStates.AnimState.zAttack);
    }

    void EndAttack()
    {

    }

    void Dying()
    {
        animStates.ChangeAnim(AnimStates.AnimState.zDead);
        animStates.isDead = true;
        //gameObject.SetActive(false);
        Invoke(nameof(RemoveCorpse), 3f);
    }

    void RemoveCorpse()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        enemyHP -= dmg;
        if (enemyHP <= 0)
            Dying();
    }
}
