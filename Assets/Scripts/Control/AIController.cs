using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 2.5f;
        [SerializeField] bool chaseDistanceGizmo = true;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        Vector3 lastKnownPlayerPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
        }



        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                // Attack state
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                // Suspicion state
                SuspicionBehaviour();
            }
            else
            {
                // Back to Guard position
                GuardBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }


        private void AttackBehaviour()
        {
            fighter.Attack(player);
            lastKnownPlayerPosition = player.transform.position;
        }

        private void SuspicionBehaviour()
        {
            mover.StartMoveAction(lastKnownPlayerPosition);
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }



        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }


        // Called by Unity
        private void OnDrawGizmosSelected()       
        {
            if (chaseDistanceGizmo)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, chaseDistance);
            }
        }
    }
}

