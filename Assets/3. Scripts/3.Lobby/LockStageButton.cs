using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockStageButton : MonoBehaviour
{
    [SerializeField] private Button MyButton;
    [SerializeField] private string Stage;

    private void OnEnable()
    {
        MyButton.interactable = false;

        string[] stringSplit = Stage.Split('_');
        int chapterNum = int.Parse(stringSplit[1]);
        int stageNum = int.Parse(stringSplit[2]);

        // �ش� ���������� Ŭ����Ǿ� �ִٸ�
        if (GameManager.MyInstance.DATA.ClearStageNum[chapterNum - 1] != 0)
            // Ŭ���� ���������� �ڽ��� �������� ���� ���ٸ� �ر�
            if (GameManager.MyInstance.DATA.ClearStageNum[chapterNum - 1] + 1 >= stageNum)
            {
                MyButton.interactable = true;
                gameObject.SetActive(false);
            }

        // é�� 2�̻󿡼� ù��° �������� �ϰ��
        if(chapterNum >= 2 && stageNum == 1)
            // ���� �������� Ŭ���� �������� ���� 10�̻��� ��� ���������� �ر��Ѵ�.
            if(GameManager.MyInstance.DATA.ClearStageNum[(chapterNum - 1) - 1] >= 10)
            {
                MyButton.interactable = true;
                gameObject.SetActive(false);
            }
    }
}
