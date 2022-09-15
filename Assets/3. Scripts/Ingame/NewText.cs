using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class NewText : MonoBehaviour
{
    [SerializeField]
    private NewTextPool.NewTextPrefabsName TextType;

    public float Value;
    private TextMeshPro text;
    private Color alpha;

    void Start()
    {
        alpha = text.color;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, Time.deltaTime, 0));
        text.color = alpha;
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f);

        while(true)
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * 10);
            if (alpha.a == 0)
                break;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator ReturnObject()
    {
        yield return new WaitForSeconds(1);

        if (NewTextPool.Instance == null)
            Destroy(gameObject);
        else
            NewTextPool.Instance.ReturnObject(this, TextType);
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
        if (Value == 0 && !TextType.Equals(NewTextPool.NewTextPrefabsName.Heal))
            text.text = "MISS";
        else
            text.text = Value.ToString();
        StartCoroutine(ReturnObject());
        StartCoroutine(FadeOut());
    }
}
