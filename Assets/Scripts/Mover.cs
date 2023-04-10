using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
		if (Input.GetMouseButton(0)) {
			MoveToCursor();
		}
		UpdateAnimator();
	}

	private void UpdateAnimator()
	{
		Vector3 velocity = navMeshAgent.velocity;
		Vector3 localVelocity = transform.InverseTransformDirection(velocity);
		float speed = localVelocity.z;
		animator.SetFloat(FORWARD_SPEED, speed);
	}

	private void MoveToCursor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		bool hasHit = Physics.Raycast(ray, out hit);
		if(hasHit) {
			navMeshAgent.destination = hit.point;
		}
	}
}
