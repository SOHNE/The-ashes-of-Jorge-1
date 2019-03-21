using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    public bool onGround = false;

    private void OnTriggerEnter(Collider other)
    {
        onGround |= other.name == "Ground";
        print(other.name);
    }

}
