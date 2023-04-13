using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		private const string ATTACK_TRIGGER = "attack";
		Transform target;
		[SerializeField] float weaponRange;
		[SerializeField] float timeBetweenAttacks;
		private float timeSinceLastAttack = Mathf.Infinity;
		Mover mover;
		Animator animator;
		ActionScheduler actionScheduler;
		
		private void Start()
		{
			mover = GetComponent<Mover>();
			actionScheduler = GetComponent<ActionScheduler>();
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
			if (target == null) {
				return;
			}
			if (GetIsInRange()) {
				mover.Cancel();
				AttackBehaviour();
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

		private void AttackBehaviour()
		{
			if (timeSinceLastAttack > timeBetweenAttacks) {
				TriggerAttack();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			animator.SetTrigger(ATTACK_TRIGGER);
		}

		// Triggered by animation
		private void Hit()
		{

		}
	}
}
