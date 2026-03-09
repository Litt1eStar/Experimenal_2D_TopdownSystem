using System.Collections;
using UnityEngine;

public class FiringControlelr : MonoBehaviour
{
    [Header("Hand Transform Reference")]
    [SerializeField] private Transform leftHand_origin;
    [SerializeField] private Transform rightHand_origin;

    [Header("Prefab Reference")]
    [SerializeField] private GameObject leftHand_attack_prefab;
    [SerializeField] private GameObject rightHand_attack_prefab;

    [Header("Cooldown Reference")]
    [SerializeField] private float leftHand_cooldown = 0.5f;
    [SerializeField] private float rightHand_cooldown = 1f;

    private bool canFire = true;
    private float firingTimer = 0f;
    private float leftHand_firingTimer = 0f;
    private float rightHand_firingTimer = 0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire)
        {
            GetClickPosition(out Vector3 direction);
            ProjectileObject projectile = CreateProjectilePrefab(leftHand_attack_prefab, leftHand_origin.position, direction);    

            if (projectile != null)
            {
                ProjectileSetup(projectile, direction);
            }

            StartCoroutine(FiringCooldown(leftHand_cooldown, leftHand_firingTimer));
        }

        if (Input.GetMouseButtonDown(1) && canFire)
        {
            GetClickPosition(out Vector3 direction);
            ProjectileObject projectile = CreateProjectilePrefab(rightHand_attack_prefab, rightHand_origin.position, direction);

            if (projectile != null)
            {
                ProjectileSetup(projectile, direction);
            }

            StartCoroutine(FiringCooldown(rightHand_cooldown, rightHand_firingTimer));
        }
    }

    private void ProjectileSetup(ProjectileObject projectile, Vector3 direction)
    {
        projectile.SetDirection(direction);

        Vector3 pos = projectile.transform.position;
        pos.z = 0f;
        projectile.transform.position = pos;
    }

    private ProjectileObject CreateProjectilePrefab(GameObject prefab, Vector3 origin, Vector3 _direction)
    {
        GameObject attackPrefab = Instantiate(
            prefab,
            origin,
            Quaternion.LookRotation(Vector3.forward, _direction)
            );

        ProjectileObject projectile = attackPrefab.GetComponent<ProjectileObject>();

        return projectile;
    }
    private IEnumerator FiringCooldown(float _firingCooldown, float _firingTimer)
    {
        _firingTimer = _firingCooldown;
        canFire = false;

        while(_firingTimer > 0f)
        {
            _firingTimer -= Time.deltaTime;
            yield return null;
        }

        canFire = true;
    }

    private void GetClickPosition(out Vector3 _direction)
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = transform.position.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3 direction = (worldPosition - transform.position).normalized;

        _direction = direction;
    }
}
