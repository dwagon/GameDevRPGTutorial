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
		private const string STOP_ATTACK_TRIGGER = "stopAttack";
		Health target;
		[SerializeField] float weaponRange;
		[SerializeField] float timeBetweenAttacks;
		[SerializeField] float weaponDamage;
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
			if(!target.IsAlive()) {
				return;
			}
			if (GetIsInRange()) {
				mover.Cancel();
				AttackBehaviour();
			} else {
				mover.MoveTo(target.transform.position);
			}
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
		}

		public void Attack(CombatTarget combatTarget)
		{
			actionScheduler.StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		public void Cancel()
		{
			animator.SetTrigger(STOP_ATTACK_TRIGGER);
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
			if (target != null) {
				target.TakeDamage(weaponDamage);
			}
		}
	}
}
