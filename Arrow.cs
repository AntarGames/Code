using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float damage;
    [SerializeField] private float accuarcy;
    [SerializeField] private float criticalChance;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private ParticleSystem missingEffect;
    [SerializeField] private ParticleSystem criticalEffect;
    private void FixedUpdate()
    {
        if (target) endPos = target.damagePoint.position;
        Movement();
    }
    public override void Movement()
    {
        if (value <= 1.0f)
        {
            middlePoint = Bezier.MiddlePoint(startPos, endPos, flightAltitude);
            currentPoint = Bezier.PointOnBezier(startPos, middlePoint, endPos, value);
            nextPoint = Bezier.PointOnBezier(startPos, middlePoint, endPos, value + step);
            direction = nextPoint - currentPoint;
            transform.forward = direction;
            transform.position = currentPoint;
            value += Time.deltaTime * flightSpeed;
        }
        else
        {
            Reached();
        }
    }
    public override void ApplyDamage()
    {
        float transformedDamage = damage;
        float criticalRate = Random.value;
        float missingRate = Random.Range(0, accuarcy);

        if (missingRate > target.Agility)
        {
            if (criticalRate < criticalChance)
            {
                transformedDamage *= damageMultiplier;
                Instantiate(criticalEffect, endPos,Quaternion.identity);
            }
            target.TakeDamage(transformedDamage, 0);
        }
        else Instantiate(missingEffect, endPos, Quaternion.identity);
    }
    public void SetInitialParameters(Enemy targetValue, float damageValue, float accuarcyValue,float criticalChanceValue,float damageMultiplierValue)
    {
        target = targetValue;
        damage = damageValue;
        accuarcy = accuarcyValue;
        criticalChance = criticalChanceValue;
        damageMultiplier = damageMultiplierValue;
        startPos = transform.position;
        endPos = target.damagePoint.position;
    }
}

