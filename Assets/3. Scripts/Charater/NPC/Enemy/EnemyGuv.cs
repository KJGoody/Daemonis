using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyGuv : EnemyBase
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
        // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        HealthBarImage.SetActive(true);
        if (knockbackDir != Vector2.zero)
            StartCoroutine(KnockBack(knockbackDir, 1));
        // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // Character TakeDamge 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
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
            // EnemyElite TeakeDamage何盒
            BossHPBar.Instance.SetBossHP(this, attackType);
            NEWText(TextType, Damage);

            if (stat.CurrentHealth <= 0)
            {
                Direction = Vector2.zero;
                myRigid2D.velocity = direction;

                // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                Player.MyInstance.SpendEXP(enemytype.EXP);
                ChangeState(new FollowState());

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
                // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                BossHPBar.Instance.BossHPBarSetActive(false);
            }
        }
        else
            NEWText(TextType);
        // Character TakeDamge 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    }

    protected override IEnumerator Death()
    {
        ParentGate.TotalGuvNum++;
        ParentGate.CurrnetGuvNum--;
        
        yield return base.Death();
    }
}
