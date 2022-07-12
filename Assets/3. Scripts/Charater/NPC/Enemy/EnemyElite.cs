using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyElite : EnemyBase
{
    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
        }

        if (Vector2.Distance(transform.position, Player.MyInstance.transform.position) > 10)
        {
            ChangeState(new IdleState());
            switch (enemytype.enemyType)
            {
                case EnemyType.EnemyTypes.Koblod_Melee:
                    MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Melee_Elite);
                    break;

                case EnemyType.EnemyTypes.Koblod_Ranged:
                    MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Ranged_Elite);
                    break;
            }

            InitializeEnemyBase();
            ParentGate.CurrentEnemyNum--;

            ParentGate.CurrnentEliteNum--;
        }

        HandleLayers();

        RegenTime += Time.deltaTime;
        if (IsAlive && RegenTime >= 1)
        {
            stat.CurrentHealth += stat.HealthRegen;
            if (stat.ManaBar != null)
                stat.CurrentMana += stat.ManaRegen;
            RegenTime = 0;
        }
    }

    public override Transform Select()
    {
        BossHPBar.Instance.BossHPBarSetActive(true, this, true);

        return base.Select();
    }

    public override void DeSelect()
    {
        BossHPBar.Instance.BossHPBarSetActive(false, this, true);

        base.DeSelect();
    }

    public override void TakeDamage(DamageType damageType, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType, AttackType attackType = AttackType.Normal)
    {
        BossHPBar.Instance.BossHPBarSetActive(true, this, false, attackType);
        // EnemyBase TakeDamage �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        HealthBarImage.SetActive(true);
        if (knockbackDir != Vector2.zero)
            StartCoroutine(KnockBack(knockbackDir, 1));
        // EnemyBase TakeDamage �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // Character TakeDamge �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        if (ChanceMaker.GetThisChanceResult_Percentage(HitPercent, MyStat.DodgePercent))
        {
            float PureDamage = pureDamage * DebuffxDamage;
            int Damage = 0;
            switch (damageType)
            {
                case DamageType.Physic:
                    Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseDefence + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
                    break;

                case DamageType.Masic:
                    Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseMagicRegist + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
                    break;

                case DamageType.True:
                    Damage = (int)Mathf.Floor((PureDamage + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
                    break;
            }

            stat.CurrentHealth -= Damage;
            // EnemyElite TeakeDamage�κ�
            BossHPBar.Instance.SetBossHP(this, attackType);
            NEWText(TextType, Damage);

            if (stat.CurrentHealth <= 0)
            {
                Direction = Vector2.zero;
                myRigid2D.velocity = direction;

                // EnemyBase TakeDamage �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                Player.MyInstance.SpendEXP(EnemyEXP);
                ChangeState(new IdleState());

                SoundManager.Instance.PlaySFXSound("BodyExploding" + Random.Range(1, 3));
                _prefabs.PlayAnimation(2);
                HealthBarImage.SetActive(false);
                _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
                transform.Find("HitBox").gameObject.SetActive(false);
                transform.Find("EnemyBody").gameObject.SetActive(false);
                myRigid2D.simulated = false;

                ItemDropManager.MyInstance.DropGold(transform, stat.Level);
                ItemDropManager.MyInstance.DropItem(transform, stat.Level);

                StartCoroutine(Death());
                ComboManager.Instance.IncreaseCombo();
                // EnemyBase TakeDamage �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                BossHPBar.Instance.BossHPBarSetActive(false);
            }
        }
        else
            NEWText(TextType);
        // Character TakeDamge �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    }

    protected override IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        SetLayersRecursively(_prefabs.transform, "None");

        yield return new WaitForSeconds(0.2f);
        SetLayersRecursively(_prefabs.transform, "Default");

        switch (enemytype.enemyType)
        {
            case EnemyType.EnemyTypes.Koblod_Melee:
                MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Melee_Elite);
                break;

            case EnemyType.EnemyTypes.Koblod_Ranged:
                MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Ranged_Elite);
                break;
        }

        InitializeEnemyBase();
        ParentGate.CurrentEnemyNum--;
        ParentGate.DeathEnemyNum++;

        ParentGate.CurrnentEliteNum--;
        ParentGate.DeathEliteNum++;
    }
}