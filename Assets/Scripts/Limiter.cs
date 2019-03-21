using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiter : MonoBehaviour
{

    private CameraFollow cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            cam.maxXAndY.x = transform.position.x;
        }
    }
}
