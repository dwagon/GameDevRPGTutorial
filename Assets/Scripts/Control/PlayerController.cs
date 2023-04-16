using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] GameObject player;
		private Mover mover;
		private Fighter fighter;
		private Health health;

		private void Start()
		{
			mover = GetComponent<Mover>();
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
		}

		private void Update()
		{
			if(!health.IsAlive()) {
				return;
			}
			if(InteractWithCombat()) {
				return;
			};
			if(InteractWithMovement()) {
				return;
			}
		}

		private bool InteractWithCombat()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
			foreach (RaycastHit hit in hits) {
				if (hit.transform.TryGetComponent<CombatTarget>(out CombatTarget target)) {
					if (!fighter.CanAttack(target.gameObject)) {
						continue;
					}
					if (Input.GetMouseButton(0)) {
						fighter.Attack(target.gameObject);
					}
					return true;
				}
			}
			return false;
		}

		private bool InteractWithMovement()
		{
			RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
			if (hasHit) {
				if (Input.GetMouseButton(0)) {
					mover.MoveTo(hit.point);
				}
				return true;
			}
			return false;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}
