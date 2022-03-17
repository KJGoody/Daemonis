using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{


    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    private Transform target; // ������ ���


    // Use this for initialization
    void Start()
    {

        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("target").transform; // Ÿ�� �̸� ���� ���ӿ�����Ʈ ã��
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
        myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��

    }
}