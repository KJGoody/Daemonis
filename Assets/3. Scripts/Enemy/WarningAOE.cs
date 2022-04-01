using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningAOE : MonoBehaviour
{
    private float alphaSpeed;
    public  float destroyTime;

    SpriteRenderer warningAOE;
    Color alpha;

    void Start()
    {
        warningAOE = GetComponent<SpriteRenderer>();
        alpha = warningAOE.color;
        alphaSpeed = 10 / destroyTime;
        Invoke("DestroyObject", destroyTime);
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        warningAOE.color = alpha;
    }

    IEnumerator FadeOut()
    {
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        yield return null;
        StartCoroutine(WaitFadeOut());
    }
    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut());
    }


    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
