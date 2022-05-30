using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 1;
    private float alphaSpeed = 10;
    private float destroyTime = 1;

    public float Damage;
    private TextMeshPro text;
    private Color alpha;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;

        if (Damage == 0)
            text.text = "MISS";
        else
            text.text = Damage.ToString();

        Invoke("DestroyObject", destroyTime);
        StartCoroutine(WaitFadeOut());
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        text.color = alpha;

    }

    private IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        yield return null;
        StartCoroutine(FadeOut());
    }
    
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
