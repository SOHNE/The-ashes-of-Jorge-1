using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour {
    public GameObject ch, ev;
    
    public void DisableElements() {
        ev.SetActive(false);
        ch.SetActive(false);
    }
}