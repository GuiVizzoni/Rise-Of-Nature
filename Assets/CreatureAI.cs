using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class CreatureAI : MonoBehaviour
    {
        public Transform player;
        public float followDistance = 10f;
        public float viewDistance = 15f;
        public float fieldOfView = 120f;
        public float wanderRadius = 10f;
        public float wanderInterval = 4f;

        private CreatureMover mover;
        private float wanderTimer;
        private Vector3 wanderTarget;

        void Start()
        {
            mover = GetComponent<CreatureMover>();
            wanderTimer = wanderInterval;
            PickNewWanderPoint();
        }

        void Update()
        {
            wanderTimer += Time.deltaTime;

            Vector3 toPlayer = player.position - transform.position;
            float distance = toPlayer.magnitude;

            bool isPlayerVisible = IsPlayerVisible(toPlayer);

            if (isPlayerVisible && distance <= followDistance)
            {
                Vector3 moveDir = (player.position - transform.position).normalized;
                Vector2 input = new Vector2(moveDir.x, moveDir.z);
                mover.SetInput(input, player.position, true, false);
            }
            else
            {
                if (wanderTimer >= wanderInterval)
                {
                    PickNewWanderPoint();
                    wanderTimer = 0f;
                }

                Vector3 toTarget = wanderTarget - transform.position;
                Vector2 input = new Vector2(toTarget.normalized.x, toTarget.normalized.z);

                mover.SetInput(input, wanderTarget, false, false);

                if (toTarget.magnitude < 1f)
                    PickNewWanderPoint();
            }
        }

        bool IsPlayerVisible(Vector3 toPlayer)
        {
            if (toPlayer.magnitude > viewDistance) return false;

            float angle = Vector3.Angle(transform.forward, toPlayer.normalized);
            if (angle > fieldOfView / 2f) return false;

            if (Physics.Raycast(transform.position + Vector3.up, toPlayer.normalized, out RaycastHit hit, viewDistance))
            {
                return hit.transform == player;
            }

            return false;
        }

        void PickNewWanderPoint()
        {
            Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
            randomDir.y = 0;
            wanderTarget = transform.position + randomDir;
        }
    }
}
