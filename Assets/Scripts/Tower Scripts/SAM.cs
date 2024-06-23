using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAM : TowerParent
{
    [Header("References")]
    [SerializeField] private Transform turretRotation; //SAve this for sprite if needed
    //[SerializeField] private TowerStats towerStats;
    [SerializeField] private LayerMask enemyMask; // add a layer mask called flying enemy to detect it on raycast
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed; //Save this for sprites if needed
    

    public Transform target;
    private float timeToFire;

    void Awake()
    {
        towerStats.SetSAM(this);
    }


    void Update()
    {
        if (target == null)
        {
            SamFindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!SamCheckTargetInRange())
        {
            target = null;
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / _fireRate)
            {
                timeToFire = 0f;
                turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
            }

        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotation.rotation = Quaternion.RotateTowards(turretRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private bool SamCheckTargetInRange()
    {
        return Vector2.Distance(target.position, firePoint.position) <= _range;
    }

    private void SamFindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, _range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(firePoint.position, _range);
    }
}
