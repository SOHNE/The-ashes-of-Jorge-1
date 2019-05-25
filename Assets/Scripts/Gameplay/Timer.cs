using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    TextMeshProUGUI arroz;

    // Start is called before the first frame update
    void Start()
    {
        arroz = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        arroz.text = string.Format("{0}", (int)Time.timeSinceLevelLoad);
    }
}
