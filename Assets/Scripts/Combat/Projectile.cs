using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming = true;
        private Health _target = null;
        private float _damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target is null)
            {
                return;
            }

            if (isHoming && _target.IsAlive())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule is null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (!_target.IsAlive()) return;
            if (_target != null && health != _target) return;
            health.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}