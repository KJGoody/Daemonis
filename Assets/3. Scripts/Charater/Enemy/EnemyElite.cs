using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class EnemyElite : EnemyBase
{
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

    public override void TakeDamage(bool IsPhysic, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType)
    {
        BossHPBar.Instance.BossHPBarSetActive(true, this);
        // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        HealthBarImage.SetActive(true);
        if (knockbackDir != Vector2.zero)
            StartCoroutine(KnockBack(knockbackDir, 1));
        // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // Character TakeDamge 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        if (ChanceMaker.GetThisChanceResult_Percentage(HitPercent, MyStat.DodgePercent))
        {
            float PureDamage = pureDamage * DebuffxDamage;
            int Damage;
            if (IsPhysic)
                Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseDefence + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
            else
                Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseMagicRegist + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));

            if (TextType.Equals(NewTextPool.NewTextPrefabsName.Critical))
                Damage *= (int)Player.MyInstance.MyStat.CriticalDamage / 100;

            stat.CurrentHealth -= Damage;
            // EnemyElite TeakeDamage何盒
            BossHPBar.Instance.SetBossHP(this);
            NEWText(TextType, Damage);

            if (stat.CurrentHealth <= 0)
            {
                Direction = Vector2.zero;
                myRigid2D.velocity = direction;

                // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
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
                // EnemyBase TakeDamage 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                BossHPBar.Instance.BossHPBarSetActive(false, this);
            }
        }
        else
            NEWText(TextType);
        // Character TakeDamge 何盒 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    }
}
