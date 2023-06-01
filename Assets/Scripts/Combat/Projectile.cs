using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 2f;
        
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
            Destroy(gameObject, maxLifeTime);
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
            speed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}