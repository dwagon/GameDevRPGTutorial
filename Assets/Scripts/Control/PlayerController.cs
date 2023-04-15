using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Movement;
using RPG.Combat;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] GameObject player;
		private Mover mover;
		private Fighter fighter;

		private void Start()
		{
			mover = GetComponent<Mover>();
			fighter = GetComponent<Fighter>();
		}

		private void Update()
		{
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
					if (!fighter.CanAttack(target)) {
						continue;
					}
					if (Input.GetMouseButtonDown(0)) {
						fighter.Attack(target);
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
				if (Input.GetMouseButtonDown(0)) {
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
