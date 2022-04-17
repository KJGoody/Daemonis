using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    private float moveSpeed = 1;
    private float alphaSpeed = 10;
    private float destroyTime = 1;

    public int Damage { get; set; }
    TextMeshPro text;
    Color alpha;

    public string TextType;


    void Start()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        switch (TextType)
        {
            case "PlayerDamage":
                alpha = new Color(194 / 255f, 31 / 255f, 31 / 255f);
                break;

            case "CriticalDamage":
                alpha = new Color(255 / 255f, 212 / 255f, 0 / 255f);
                break;

        }
        text.text = Damage.ToString();
        Invoke("DestroyObject", destroyTime);
        StartCoroutine(WaitFadeOut());
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        // alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;

    }
    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
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
