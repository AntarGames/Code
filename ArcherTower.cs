using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ArcherTower : Tower
{
    [SerializeField] private Animator archerAnimator;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Arrow arrow;
    [SerializeField] private float damage;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float criticalChance;
    [SerializeField] private float accuarcy;

    public override void Attack()
    {
        archerAnimator.Play("Shot");
        Arrow arrowClone = Instantiate(arrow, attackPosition.position, Quaternion.identity,transform);
        arrowClone.SetInitialParameters(target, damage, accuarcy, criticalChance, damageMultiplier);
    }

}
