using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control {
	public class AIController : MonoBehaviour
	{
		private const string PLAYER_TAG = "Player";
		[SerializeField] float chaseDistance = 5f;
		private GameObject player;
		private Fighter fighter;
		private Health health;
		private Mover mover;
		private ActionScheduler actionScheduler;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceLastArrivedAtWaypoint = Mathf.Infinity;
		private int currentWaypointIndex;
		[SerializeField] float suspicionTime;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointDwellTime = 2f;
		private float waypointTolerance = 0.5f;

		private void Start()
		{
			guardPosition = transform.position;
			player = GameObject.FindWithTag(PLAYER_TAG);
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			actionScheduler = GetComponent<ActionScheduler>();
			currentWaypointIndex = 0;
		}

		private void Update()
		{
			if (!health.IsAlive()) {
				return;
			}
			if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
				timeSinceLastSawPlayer = 0f;
				AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer < suspicionTime) {
				SuspicionBehaviour();
			}
			else {
				PatrolBehaviour();
			}
			UpdateTimers();
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceLastArrivedAtWaypoint += Time.deltaTime;
		}

		private void AttackBehaviour()
		{
			fighter.Attack(player);
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;
			if (patrolPath != null) {
				if (AtWaypoint()) {
					CycleWaypoint();
					timeSinceLastArrivedAtWaypoint = 0f;
				}
				nextPosition = GetCurrentWaypoint();
			}
			if (timeSinceLastArrivedAtWaypoint > waypointDwellTime) {
				mover.StartMoveAction(nextPosition);
			}
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint(currentWaypointIndex);
		}

		private void SuspicionBehaviour()
		{
			actionScheduler.CancelCurrentAction();
		}

		private bool InAttackRangeOfPlayer()
		{
			float dist = Vector3.Distance(player.transform.position, transform.position);
			if(dist < chaseDistance) {
				return true;
			}
			return false;
		}

		// Called by Unity
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}
