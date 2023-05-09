using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponSO : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float weaponRange;
        [SerializeField] private float weaponDamage;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetRange()
        {
            Debug.Log("Get Range = " + weaponRange);
            return weaponRange;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
    }
}
