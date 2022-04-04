using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningArea : MonoBehaviour
{
    private float alphaNum;
    private float time = 0f;
    public  float destroyTime;

    SpriteRenderer warningArea;

    void Start()
    {
        warningArea = GetComponent<SpriteRenderer>();
        Invoke("DestroyObject", destroyTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color waringAreaColor = warningArea.color;

        while (waringAreaColor.a > 0f)
        {
            time += Time.deltaTime / destroyTime;
            waringAreaColor.a = Mathf.Lerp(1, 0, time);
            warningArea.color = waringAreaColor;
            yield return null;
        }
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
