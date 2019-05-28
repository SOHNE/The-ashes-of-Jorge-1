using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    public TextMeshProUGUI Clock;

    // Update is called once per frame
    void Update() {
        Clock.text = string.Format("{0}", (int) Time.timeSinceLevelLoad);
    }
}