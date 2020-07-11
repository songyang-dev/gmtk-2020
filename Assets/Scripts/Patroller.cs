using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script lets the AI (navmesh agent) patrol using patrolTargets which is checked every frame in Update
/// and run through a coroutine to move to the next target.
/// 
/// It also allows the AI to see the player (or target) using a raycast, and if it hits the target, then it can
/// see player and chases. When in certain stopping distance, it attacks the player.
/// 
/// The AI stores the last known position of the target so that it continually follows the player.
/// </summary>
public class Patroller : MonoBehaviour
{

    #region References to unity components
    NavMeshAgent agent;
    Animator anim;
    #endregion

    /// <summary>
    /// Transform of the hunted player
    /// </summary>
    public Transform target;

    /// <summary>
    /// Last seen position of the target
    /// </summary>
    Vector3 lastKnownPosition;

    /// <summary>
    /// Eye of the patroller ? #TODO
    /// </summary>
    public Transform eye;

    /// <summary>
    /// Whether the patroller is patrolling
    /// </summary>
    bool patrolling;
    
    /// <summary>
    /// Waypoints of the patrol trajectory
    /// </summary>
    public Transform[] patrolTargets;

    /// <summary>
    /// Index of the waypoint of interest in the above array ? #TODO
    /// </summary>
    private int destPoints;

    /// <summary>
    /// Whether the patroller is within a agent.stoppingDistance of the target
    /// </summary>
    bool arrived;

    /// <summary>
    /// Links the references to the intended components
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lastKnownPosition = transform.position; // Questionable? #TODO
    }

    /// <summary>
    /// Determines if the patroller can see the target using a ray cast
    /// </summary>
    /// <returns>True if the target (player) can be seen</returns>
    bool CanSeeTarget()
    {
        bool canSee = false;
        Ray ray = new Ray(eye.position, target.transform.position - eye.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != target)
            {
                canSee = false;
            }
            else
            {
                lastKnownPosition = target.transform.position;
                canSee = true;
            }
        }
        return canSee;
    }

    /// <summary>
    /// Decides what the patroller should do based on physical observation
    /// </summary>
    void Update()
    {
        // not clear what happens when patrolling is false and no target is seen

        if (agent.pathPending)
            return;

        if (patrolling)
        {
            Patrol();
        }
        if (CanSeeTarget())
        {
            EngageTarget();
        }
        else
        {
            ChaseTarget();

        }
        // ? #TODO
        anim.SetFloat("Forward", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// Sends the patroller after the target's last known position.
    /// When reached that position, the patroller resumes standard patrolling
    /// if nothing else happens.
    /// </summary>
    private void ChaseTarget()
    {
        anim.SetBool("Attack", false);
        if (!patrolling)
        {

            agent.SetDestination(lastKnownPosition);
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                Debug.Log("works"); // To be removed #TODO
                patrolling = true;
                StartCoroutine(GoToNextPoint());
            }
        }
    }

    /// <summary>
    /// Chases after the target and attacks if close enough
    /// </summary>
    private void EngageTarget()
    {
        agent.SetDestination(target.transform.position);
        patrolling = false;
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Walks on a given patrol sequence of waypoints
    /// </summary>
    private void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            if (!arrived)
            {
                arrived = true;
                StartCoroutine(GoToNextPoint());
            }
        }
        else
        {
            arrived = false;
        }
    }

    /// <summary>
    /// Resumes patrolling after a delay.
    /// I suggest making the delay a parameter! #TODO
    /// </summary>
    /// <returns></returns>
    IEnumerator GoToNextPoint()
    {
        if (patrolTargets.Length == 0)
        {
            yield break;
        }

        patrolling = true;
        yield return new WaitForSeconds(2f); // explain this delay of 2 seconds #TODO
        arrived = false;

        // what is happening here #TODO
        agent.destination = patrolTargets[destPoints].position;
        destPoints = (destPoints + 1) % patrolTargets.Length;
    }
}
