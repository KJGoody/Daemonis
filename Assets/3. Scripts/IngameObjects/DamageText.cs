using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 1;
    private float alphaSpeed = 10;

    [SerializeField]
    private DamageTextPool.DamageTextPrefabsName TextType;

    public float Damage;
    private TextMeshPro text;
    private Color alpha;

    void Start()
    {
        alpha = text.color;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        text.color = alpha;

    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f);

        float time = 0;
        while(time < 1)
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator ReturnObject()
    {
        yield return new WaitForSeconds(1);
        DamageTextPool.Instance.ReturnObject(this, TextType);
    }

    public void PositioningDamageText(Transform Position)
    {
        transform.SetParent(Position);
        transform.position = new Vector3(Position.position.x, Position.position.y + 1);
    }

    public void InitializeDamageText()
    {
        alpha.a = 1;
        text = GetComponent<TextMeshPro>();
        if (Damage == 0)
            text.text = "MISS";
        else
            text.text = Damage.ToString();
        StartCoroutine(ReturnObject());
        StartCoroutine(FadeOut());
    }
}
