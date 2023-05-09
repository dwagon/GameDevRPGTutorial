using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement {
	public class Mover : MonoBehaviour, IAction, ISaveable
	{
		private const string FORWARD_SPEED = "forwardSpeed";
		[SerializeField] private Transform target;
		[SerializeField] private float maxSpeed = 6f;
		private NavMeshAgent _navMeshAgent;
		private Animator _animator;
		private Health _health;

		private void Start()
		{
			_animator = GetComponent<Animator>();
			_health = GetComponent<Health>();
		}

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
		}

		private void Update()
		{
			_navMeshAgent.enabled = _health.IsAlive();
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = _navMeshAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;
			_animator.SetFloat(FORWARD_SPEED, speed);
		}

		public void StartMoveAction(Vector3 destination, float speedFraction)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			MoveTo(destination, speedFraction);
		}

		public void MoveTo(Vector3 destination, float speedFraction)
		{
			_navMeshAgent.destination = destination;
			_navMeshAgent.isStopped = false;
			_navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
		}

		public void Cancel()
		{
			_navMeshAgent.isStopped = true;
		}

		public object CaptureState()
		{
			return new SerializableVector3(transform.position);
		}

		public void RestoreState(object state)
		{
			SerializableVector3 position = (SerializableVector3)state;
			_navMeshAgent.enabled = false;
			transform.position = position.ToVector();
			_navMeshAgent.enabled = true;
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}
		
	}
}
