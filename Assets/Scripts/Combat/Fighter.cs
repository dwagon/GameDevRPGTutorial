using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour
	{
		Transform target;
		[SerializeField] float weaponRange;
		Mover mover;

		private void Start()
		{
			mover = GetComponent<Mover>();
		}

		private void Update()
		{
			if (target == null) {
				return;
			}
			if (GetIsInRange()) {
				mover.Stop();
			} else {
				mover.MoveTo(target.position);
			}
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.position) < weaponRange;
		}

		public void Attack(CombatTarget combatTarget)
		{
			target = combatTarget.transform;
		}

		public void Cancel()
		{
			target = null;
		}
	}
}
