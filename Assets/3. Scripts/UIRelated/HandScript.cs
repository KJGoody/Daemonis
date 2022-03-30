using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    // 싱글톤
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    // IMoveable은 Spell에서 상속받는다.
    public IMoveable MyMoveable { get; set; }

    private Image icon;

    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        // 마우스를 따라 아이콘이 이동한다.
        icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        Debug.Log("Take");
        // 클릭한 스킬 아이콘 정보를 Icon 에 담는다.
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;

    }

    public
     IMoveable Put()
    {
        IMoveable tmp=MyMoveable;
        // 복사한 스킬 아이콘 정보를 null 로 만든다.
        MyMoveable=null;
        // 복사된 아이콘을 투명하게 만든다.
        icon.color=new Color(0,0,0,0);
        // 복사한 스킬의 아이콘 정보를 전달한다.
        return tmp;
    }
    
}