using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using RPG.Core;

namespace RPG.Movement {
	public class Mover : MonoBehaviour, IAction
	{
		private const string FORWARD_SPEED = "forwardSpeed";
		[SerializeField] private Transform target;
		[SerializeField] private float maxSpeed = 6f;
		private NavMeshAgent navMeshAgent;
		private Animator animator;
		private ActionScheduler actionScheduler;
		private Health health;

		private void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			animator = GetComponent<Animator>();
			actionScheduler = GetComponent<ActionScheduler>();
			health = GetComponent<Health>();
		}

		private void Update()
		{
			navMeshAgent.enabled = health.IsAlive();
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = navMeshAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;
			animator.SetFloat(FORWARD_SPEED, speed);
		}

		public void StartMoveAction(Vector3 destination, float speedFraction)
		{
			actionScheduler.StartAction(this);
			MoveTo(destination, speedFraction);
		}

		public void MoveTo(Vector3 destination, float speedFraction)
		{
			navMeshAgent.destination = destination;
			navMeshAgent.isStopped = false;
			navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
		}

		public void Cancel()
		{
			navMeshAgent.isStopped = true;
		}
	}
}
