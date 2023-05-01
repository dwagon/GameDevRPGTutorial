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
		[SerializeField] private GameObject weaponPrefab = null;
		[SerializeField] private Transform handTransform = null;
		private float timeSinceLastAttack = Mathf.Infinity;
		Mover mover;
		Animator animator;
		ActionScheduler actionScheduler;
		
		private void Start()
		{
			mover = GetComponent<Mover>();
			actionScheduler = GetComponent<ActionScheduler>();
			animator = GetComponent<Animator>();
			
			SpawnWeapon();
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
				float fullSpeed = 1.0f;
				mover.MoveTo(target.transform.position, fullSpeed);
			}
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if (combatTarget == null) {
				return false;
			}
			Health combatTargetHealth = combatTarget.GetComponent<Health>();

			if(combatTargetHealth!= null && !combatTargetHealth.IsAlive()) {
				return false;
			}
			return true;
		}

		public void Attack(GameObject combatTarget)
		{
			actionScheduler.StartAction(this);
			target = combatTarget.GetComponent<Health>();

		}

		public void Cancel()
		{
			animator.SetTrigger(STOP_ATTACK_TRIGGER);
			animator.ResetTrigger(ATTACK_TRIGGER);
			target = null;
			mover.Cancel();
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform);
			if (timeSinceLastAttack > timeBetweenAttacks) {
				TriggerAttack();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			animator.ResetTrigger(STOP_ATTACK_TRIGGER);
			animator.SetTrigger(ATTACK_TRIGGER);
		}

		// Triggered by animation
		private void Hit()
		{
			if (target != null) {
				target.TakeDamage(weaponDamage);
			}
		}

		private void SpawnWeapon()
		{
			Instantiate(weaponPrefab, handTransform);
		}
	}
}
