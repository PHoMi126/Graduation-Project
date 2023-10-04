using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float range; //radius of sphere
    [SerializeField] Transform centrePoint; //centre of the area the agent wants to move around in
                                            //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    [SerializeField] AnimStates animStates;
    [SerializeField] float waitTimer = 0f;

    bool isDead;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        waitTimer += Time.deltaTime;
        if (animStates != null)
        {
            if (animStates.animator.GetCurrentAnimatorStateInfo(0).IsName("zDead"))
            {
                Invoke(nameof(Dead), 2.2f);
                agent.isStopped = true;
            }
            else
            {
                if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                {
                    //animStates.ChangeAnim(AnimStates.AnimState.zIdle);

                    if (RandomPoint(centrePoint.position, range, out Vector3 point) && waitTimer >= 4f) //pass in our centre point and radius of area
                    {
                        waitTimer = 0f;
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        agent.SetDestination(point);
                    }
                }

                /* animStates.attackTime -= Time.deltaTime;
                if (animStates != null && animStates.attackTime <= 0f)
                {
                    if (animStates.animator.GetCurrentAnimatorStateInfo(0).IsName("zIdle"))
                    {
                        Attack();
                    }
                } */
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void Attack()
    {

    }

    void EndAttack()
    {

    }

    void Dead()
    {
        animStates.ChangeAnim(AnimStates.AnimState.zDead);
        isDead = true;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
