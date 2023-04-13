using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		Transform target;
		[SerializeField] float weaponRange;
		Mover mover;
		ActionScheduler actionScheduler;
		

		private void Start()
		{
			mover = GetComponent<Mover>();
			actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Update()
		{
			if (target == null) {
				return;
			}
			if (GetIsInRange()) {
				mover.Cancel();
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
			actionScheduler.StartAction(this);
			target = combatTarget.transform;
		}

		public void Cancel()
		{
			target = null;
		}
	}
}
