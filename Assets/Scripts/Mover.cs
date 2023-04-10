using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
	[SerializeField] private Transform target;
	private NavMeshAgent navMeshAgent;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			MoveToCursor();
		}
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
