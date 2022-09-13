using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedSkillImage : MonoBehaviour
{
    [SerializeField]
    private int LockedLevel;

    private void Update()
    {
        UnLockedSkillImage();
    }

    public void UnLockedSkillImage()
    {
        if (Player.MyInstance.MyStat.Level >= LockedLevel)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
