using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

namespace RPG.Movement {
	public class Mover : MonoBehaviour
	{
		private const string FORWARD_SPEED = "forwardSpeed";
		[SerializeField] private Transform target;
		private NavMeshAgent navMeshAgent;
		private Animator animator;

		private void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = navMeshAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;
			animator.SetFloat(FORWARD_SPEED, speed);
		}

		public void MoveTo(Vector3 destination)
		{
			navMeshAgent.destination = destination;
		}
	}
}
