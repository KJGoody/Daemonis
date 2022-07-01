using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChampion : EnemyBase
{
    [SerializeField]
    private string EnemyName;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TakeDamage(bool IsPhysic, float HitPercent, float PureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType)
    {
        BossHPBar.Instance.BossHPBarSetActive(true, this);

        base.TakeDamage(IsPhysic, HitPercent, PureDamage, FromLevel, knockbackDir, TextType);
       
        if(stat.CurrentHealth <= 0)
        {
            BossHPBar.Instance.BossHPBarSetActive(true, this);
        }
    }
}
