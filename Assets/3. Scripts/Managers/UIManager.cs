using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup[] menu; // �޴�â 0:ĳ���� 1:��ų 2:�κ��丮 3:�ɼ� 4:�޴�����Ʈ
    [SerializeField]
    private Image[] menuImage; //�޴� �̹���
    [SerializeField]
    private Sprite[] menuNormalImage; //�޴� ��� �̹���
    [SerializeField]
    private Sprite[] menuActiveImage; //�޴� Ȱ��ȭ �̹���
    [SerializeField]
    private CanvasGroup bigMap; // ��ü��

    void Update()
    {
        // ���߿� Ű���� ����Ű
        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            OpenClose(menu[0]);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            OpenClose(menu[1]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenClose(menu[2]);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenClose(menu[3]);
        }
    }
    public void BigMapOnOff() // �� �¿���
    {
        bigMap.alpha = bigMap.alpha > 0 ? 0 : 1;
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        // �������� UI�� ���ų� Ų��.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        switch (canvasGroup.name)
        {
            case "Charactor":
                if (canvasGroup.alpha == 1)
                    menuImage[0].sprite = menuActiveImage[0];
                else if(canvasGroup.alpha == 0)
                    menuImage[0].sprite = menuNormalImage[0];

                if (menu[2].alpha != 1 || menu[0].alpha != 1) // ĳ����â�� ������â�� ��� ����������
                    HandScript.MyInstance.Close_UE_Panel(); // ����������â�� ����
                break;

            case "SpellBook":
                if (canvasGroup.alpha == 1)
                    menuImage[1].sprite = menuActiveImage[1];
                else if (canvasGroup.alpha == 0)
                    menuImage[1].sprite = menuNormalImage[1];

                HandScript.MyInstance.ResetSelect(); // ���� ��ų �ʱ�ȭ
                break;

            case "Inventory":
                if (canvasGroup.alpha == 1)
                    menuImage[2].sprite = menuActiveImage[2];
                else if (canvasGroup.alpha == 0)
                    menuImage[2].sprite = menuNormalImage[2];

                HandScript.MyInstance.Close_SI_Panel(); // ���þ�����â ����
                if (menu[0].alpha != 1) // ĳ����â�� ����������
                    HandScript.MyInstance.Close_UE_Panel(); // ���� ������â ����
                break;

            case "Option":
                if (canvasGroup.alpha == 1)
                    menuImage[3].sprite = menuActiveImage[3];
                else if (canvasGroup.alpha == 0)
                    menuImage[3].sprite = menuNormalImage[3];
                break;

            case "MenuPanel":
                if (canvasGroup.alpha == 1)
                    menuImage[4].sprite = menuActiveImage[4];
                else if (canvasGroup.alpha == 0)
                    menuImage[4].sprite = menuNormalImage[4];
                break;
        }

        // UI �� Ŀ������ �� �����ɽ�Ʈ �浹�� �ǵ��� �����
        // UI �� �������� �� �����ɽ�Ʈ �浹�� ���õǾ� �ٸ� ����(�� ���� ��)��
        // �� �� �ְ� �����.
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
    }

    public void UpdateStackSize(IStackable stackable)
    {
        if (stackable.MyCount > 1)
        {
            // �ش� ������ ��ø���� ǥ���ϱ�
            stackable.MyStackText.text = stackable.MyCount.ToString();
            stackable.MyStackText.color = Color.white;
            (stackable as IClickable).MyIcon.color = Color.white;
        }

        else
        {
            // �ش� ������ �ؽ�Ʈ �����ϰ� �����
            stackable.MyStackText.color = new Color(0, 0, 0, 0);
        }

        if (stackable.MyCount == 0)
        {
            // �ش� ������ ������ �����ϰ� �����
            (stackable as IClickable).MyIcon.color = new Color(0, 0, 0, 0);

            // �ش� ������ �ؽ�Ʈ �����ϰ� �����
            stackable.MyStackText.color = new Color(0, 0, 0, 0);

        }
    }
}
