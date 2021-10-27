using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private int buildCost;
    [SerializeField] private int upgradeCost;
    [SerializeField] private int removeCost;
    [SerializeField] private float actionRadius;
    [SerializeField] private float cooldown;
    [SerializeField] protected Enemy target;
    [SerializeField] private Transform hero;
    [SerializeField] private Mesh circleMesh;

    private Point[] occupiedPoints;
    public Tower nextLvlTower;
    protected bool isStand;
    public bool TargetIsOut()
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, Target.Position) > actionRadius) return true;
            else return false;
        }
        else return false;
    }
    public int BuildCost { get { return buildCost; } }
    public int UpgradeCost { get { return upgradeCost; } }
    public int RemoveCost { get { return removeCost; } }
    public Enemy Target { get { return target; } set { target = value; } }
    public Point[] OccupiedPoints { set { occupiedPoints = value; } }


    private float nextAttack;

    private void FixedUpdate()
    {
        if (TargetIsOut()) target = null;
        if (!Target) Target = GetClosestTarget();
        else
        {
            if (Time.time > nextAttack)
            {
                Attack();
                nextAttack = Time.time + cooldown;
            }
            LookTarget();
        }
    }

    public void Stand()
    {
        isStand = true;
    }
    public void Upgrade()
    {
        if (nextLvlTower)
        {
            Tower tower = Instantiate(nextLvlTower, transform.position, Quaternion.identity);
            tower.occupiedPoints = occupiedPoints;
            tower.Stand();
            Destroy(gameObject);
        }
    }
    public void Remove()
    {
        ObstacleController.ClearObstacles(occupiedPoints);
        Destroy(gameObject);
    }
    public abstract void Attack();
    public Enemy GetClosestTarget()
    {
        if (!isStand) return null;
        Transform closest = null;
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, actionRadius);
        float distance = Mathf.Infinity;
        foreach(Collider nearby in nearbyObjects)
        {
            if (nearby.tag == "Enemy")
            {
                Vector3 offcet = nearby.transform.position - transform.position;
                float curDistance = offcet.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = nearby.transform;
                    distance = curDistance;
                }
            }
        }
        if (closest) return closest.GetComponent<Enemy>();
        else return null;

    }

    public void LookTarget()
    {
        if (hero)
        {
            Vector3 dir = Target.Position - hero.position;
            Vector3 projDir = Vector3.ProjectOnPlane(dir, Vector3.up);
            hero.forward = Vector3.MoveTowards(hero.forward, projDir, Time.deltaTime * 10);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,actionRadius);
    }
}
