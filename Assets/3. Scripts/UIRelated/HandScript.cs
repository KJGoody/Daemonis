using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    // �̱���
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

    // IMoveable�� Spell���� ��ӹ޴´�.
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
        // ���콺�� ���� �������� �̵��Ѵ�.
        icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        Debug.Log("Take");
        // Ŭ���� ��ų ������ ������ Icon �� ��´�.
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;

    }

    public
     IMoveable Put()
    {
        IMoveable tmp=MyMoveable;
        // ������ ��ų ������ ������ null �� �����.
        MyMoveable=null;
        // ����� �������� �����ϰ� �����.
        icon.color=new Color(0,0,0,0);
        // ������ ��ų�� ������ ������ �����Ѵ�.
        return tmp;
    }
    
}