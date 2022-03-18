using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private NPC currentTarget;
    void Update()
    {
        ClickTarget();
    }
    private void ClickTarget()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero, Mathf.Infinity, 64); // 64 = Clickable 레이어 번호
            if (hit.collider != null)
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = hit.collider.GetComponent<NPC>();

                player.myTarget = currentTarget.Select();
                //if (hit.collider.CompareTag("Enemy"))
                //{
                //    player.myTarget = hit.transform.GetChild(0);
                //    Debug.Log(player.myTarget.tag);
                //}

                //else
                //{
                //    player.myTarget = null;
                //}
            }
          else
          {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
 
                currentTarget = null;
                player.myTarget = null;
          }
        }
    }
}