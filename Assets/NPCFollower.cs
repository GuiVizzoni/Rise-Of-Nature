using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollower : MonoBehaviour
{
    public Transform player;
    public float followDistance = 10f;           // Distância máxima para seguir
    public float fieldOfViewAngle = 120f;        // Ângulo de visão
    public float viewDistance = 15f;             // Alcance de visão
    public float wanderRadius = 20f;             // Alcance da patrulha
    public float wanderTime = 5f;                // Tempo entre mudanças de destino

    private NavMeshAgent agent;
    private float wanderTimer;
    private bool playerInSight = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;
    }

    void Update()
    {
        wanderTimer += Time.deltaTime;

        // Verifica se o jogador está dentro do campo de visão e distância
        playerInSight = IsPlayerVisible();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerInSight && distanceToPlayer <= followDistance)
        {
            agent.SetDestination(player.position);
        }
        else if (wanderTimer >= wanderTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0;
        }
    }

    bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfViewAngle / 2f && directionToPlayer.magnitude <= viewDistance)
        {
            Ray ray = new Ray(transform.position + Vector3.up, directionToPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
            {
                if (hit.transform == player)
                    return true;
            }
        }
        return false;
    }

    // Gera uma posição aleatória válida no NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        if (NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask))
        {
            return navHit.position;
        }
        return origin;
    }
}
