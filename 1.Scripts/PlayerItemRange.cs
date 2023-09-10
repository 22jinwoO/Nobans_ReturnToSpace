using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemRange : MonoBehaviour
{
    [SerializeField]
    Player playerCs;
    // Start is called before the first frame update
    void Start()
    {
        playerCs=GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
