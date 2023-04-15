using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control {
	public class AIController : MonoBehaviour
	{
		private const string PLAYER_TAG = "Player";
		[SerializeField] float chaseDistance = 5f;
		private GameObject player;
		private Fighter fighter;
		private Health health;


		private void Start()
		{
			player = GameObject.FindWithTag(PLAYER_TAG);
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
		}

		private void Update()
		{
			if (!health.IsAlive()) {
				return;
			}
			if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
				fighter.Attack(player);
			}
			else {
				fighter.Cancel();
			}
		}

		private bool InAttackRangeOfPlayer()
		{
			float dist = Vector3.Distance(player.transform.position, transform.position);
			if(dist < chaseDistance) {
				return true;
			}
			return false;
		}
	}
}
