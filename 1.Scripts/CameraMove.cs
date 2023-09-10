using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("�÷��̾� ��ġ Ʈ������")]
    public Transform player;

    // �÷��̾� ��ũ��Ʈ
    private Player playerCs;

    void Start()
    {
        playerCs = player.GetComponent<Player>();
    }

    void Update()
    {
        if (playerCs.transform.position.y>4f)   // �÷��̾ ������ �ϱ� �����ߴٸ�
        {
            Vector3 targetPos = new Vector3(0, player.transform.position.y + 1.5f, -10);
            Vector3 myPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            if (playerCs.rigd.velocity.y < 1.5f)
            {
                transform.position = Vector3.Lerp(myPos, targetPos, Time.deltaTime * 0.8f);
            }
            else if (playerCs.rigd.velocity.y >= 1.5f)
            {
                transform.position = Vector3.Lerp(myPos, targetPos, Time.deltaTime * 2f);
            }
        }
        

    }
}
