using UnityEngine;

public class BossUI : MonoBehaviour {
    public void TurnOn() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }
}