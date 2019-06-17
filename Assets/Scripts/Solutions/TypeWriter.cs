using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// attach to UI Text component (with the full text already there)

public class TypeWriter : MonoBehaviour {
    private Text txt;
    private string story;

    private void Awake() {
        txt = GetComponent<Text>();
        story = txt.text;
        txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine(PlayText());
    }

    private IEnumerator PlayText() {
        foreach (char c in story) {
            txt.text += c;
            yield return new WaitForSeconds(.125f);
        }
    }

}