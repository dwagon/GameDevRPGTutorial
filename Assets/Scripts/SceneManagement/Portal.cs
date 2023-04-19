using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
	public class Portal : MonoBehaviour
	{
		enum DestinationIdentifier
		{
			A, B, C, D, E
		}
		[SerializeField] int sceneToLoad = -1;
		[SerializeField] Transform spawnPoint;
		[SerializeField] DestinationIdentifier destination;

		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Player") {
				StartCoroutine(Transition());			}
		}

		private IEnumerator Transition()
		{
			Debug.Log("Scene to load " + sceneToLoad);
			if(sceneToLoad < 0) {
				Debug.LogError("Scene to load not set");
				yield break;
			}

			DontDestroyOnLoad(gameObject);
			yield return SceneManager.LoadSceneAsync(sceneToLoad);
			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);
			Destroy(gameObject);
		}

		private void UpdatePlayer(Portal otherPortal)
		{
			GameObject player = GameObject.FindWithTag("Player");
			player.GetComponent<NavMeshAgent>().enabled = false;
			player.transform.position = otherPortal.spawnPoint.position;
			player.transform.rotation = otherPortal.spawnPoint.rotation;
			player.GetComponent<NavMeshAgent>().enabled = true;
		}

		private Portal GetOtherPortal()
		{
			foreach (Portal portal in FindObjectsOfType<Portal>()) {
				if (portal == this) {
					continue;
				}
				if (portal.destination != destination) {
					continue;
				}
				return portal;
			}
			return null;
		}
	}
}
