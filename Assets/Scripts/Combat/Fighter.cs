using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.Serialization;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		private const string ATTACK_TRIGGER = "attack";
		private const string STOP_ATTACK_TRIGGER = "stopAttack";
		[SerializeField] float timeBetweenAttacks;
		[FormerlySerializedAs("handTransform")] [SerializeField] private Transform rightHandTransform = null;
		[SerializeField] private Transform leftHandTransform = null;

		[FormerlySerializedAs("weapon")] [SerializeField] private WeaponSO defaultWeapon = null;
		Health _target;
		private float _timeSinceLastAttack = Mathf.Infinity;

		private WeaponSO currentWeapon = null;

		private void Start()
		{
			EquipWeapon(defaultWeapon);
		}

		private void Update()
		{
			_timeSinceLastAttack += Time.deltaTime;
			if (_target == null) {
				return;
			}
			Debug.Log(this.name + ": Target =" + _target.name);
			if(!_target.IsAlive()) {
				Debug.Log(this.name + ": Target is dead");
				return;
			}
			if (GetIsInRange()) {
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
			} else {
				float fullSpeed = 1.0f;
				GetComponent<Mover>().MoveTo(_target.transform.position, fullSpeed);
			}
		}

		public void Cancel()
		{
			GetComponent<Animator>().SetTrigger(STOP_ATTACK_TRIGGER);
			GetComponent<Animator>().ResetTrigger(ATTACK_TRIGGER);
			_target = null;
			GetComponent<Mover>().Cancel();
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, _target.transform.position) < currentWeapon.GetRange();
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if (combatTarget == null) {
				return false;
			}
			Health combatTargetHealth = combatTarget.GetComponent<Health>();

			if(combatTargetHealth != null && !combatTargetHealth.IsAlive()) {
				return false;
			}
			return true;
		}

		public void Attack(GameObject combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			Debug.Log("A " + this.name);
			_target = combatTarget.GetComponent<Health>();
			Debug.Log(this.name + " attack " + _target.name);
		}

		private void AttackBehaviour()
		{
			transform.LookAt(_target.transform);
			if (_timeSinceLastAttack > timeBetweenAttacks) {
				TriggerAttack();
				_timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			GetComponent<Animator>().ResetTrigger(STOP_ATTACK_TRIGGER);
			GetComponent<Animator>().SetTrigger(ATTACK_TRIGGER);
		}

		// Triggered by animation
		private void Hit()
		{
			if (_target != null) {
				_target.TakeDamage(currentWeapon.GetDamage());
			}
		}

		public void EquipWeapon(WeaponSO weapon)
		{
			currentWeapon = weapon;
			weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
		}
	}
}
