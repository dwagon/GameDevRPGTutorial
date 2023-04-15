using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
	public class Health : MonoBehaviour
	{
		private const string DIE_TRIGGER = "die";
		[SerializeField] float healthPoints;
		private bool isAlive;

		public void Start()
		{
			isAlive = true;
		}

		public bool IsAlive()
		{
			return isAlive;
		}

		public void TakeDamage(float damage)
		{
			healthPoints -= damage;
			if(healthPoints <= 0) {
				healthPoints = 0;
				if (isAlive) {
					Die();
				}
			}
		}

		public void Die()
		{
			GetComponent<Animator>().SetTrigger(DIE_TRIGGER);
			GetComponent<ActionScheduler>().CancelCurrentAction();
			isAlive = false;
		}

	}
}
