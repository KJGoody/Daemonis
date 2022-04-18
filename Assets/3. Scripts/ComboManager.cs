using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ComboView;
    private Text comboNum;
    private Text comboText;

    private Animator myAnim;

    private int currentCombo = 0;

    private Coroutine nowCoroutine;

    void Awake()
    {
        ComboView.SetActive(false);
        myAnim = ComboView.GetComponent<Animator>();
        comboNum = ComboView.transform.Find("ComboNum").gameObject.GetComponent<Text>();
        comboText = ComboView.transform.Find("ComboText").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        
    }

    public void IncreaseCombo()
    {
        if(nowCoroutine != null)
            StopCoroutine(nowCoroutine);

        currentCombo += 10;
        comboNum.text = string.Format("{0}", currentCombo);

        if (currentCombo > 1)
        {
            ComboView.SetActive(true);

            switch (currentCombo)
            {
                case 50:
                    StartCoroutine(comboUp());
                    break;

                case 300:
                    StartCoroutine(comboUp());
                    break;
            }

            nowCoroutine = StartCoroutine(ComboTimer());
        }
    }

    private IEnumerator comboUp()
    {
        if(currentCombo > 299)
        {
            comboNum.color = new Color(147 / 255f, 0 / 255f, 1 / 255f);
            comboNum.fontSize = 110;

            comboText.color = new Color(147 / 255f, 0 / 255f, 1 / 255f);
            comboNum.fontSize = 110;
            comboText.text = "COMBO!!!";
            myAnim.SetTrigger("TextShake");

            ComboView.GetComponent<RectTransform>().anchoredPosition += new Vector2(50f, 0);
        }
        else
        {
            comboNum.color = new Color(255 / 255f, 165 / 255f, 0f);
            comboNum.fontSize = 75;

            comboText.color = new Color(255 / 255f, 165 / 255f, 0f);
            comboNum.fontSize = 75;
            comboText.text = "COMBO!!";
            myAnim.SetTrigger("TextShake");

            ComboView.GetComponent<RectTransform>().anchoredPosition += new Vector2(50f, 0);
        }

        yield return new WaitForSeconds(1f);
        myAnim.SetTrigger("EndShake");
    }

    private IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(5f);
        ComboView.SetActive(false);
        currentCombo = 0;

        comboNum.color = Color.white;
        comboNum.fontSize = 50;

        comboText.color = Color.white;
        comboText.fontSize = 50;
        comboText.text = "COMBO!";
    }
}
