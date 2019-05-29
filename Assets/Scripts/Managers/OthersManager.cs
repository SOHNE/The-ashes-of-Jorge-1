using System.Collections.Generic;
using UnityEngine;

public class OthersManager : MonoBehaviour {
    public List<Enemy> Enemies;
    public List<Enemy> Attacking;

    private void FixedUpdate() {
        Catch();
        Do();
    }

    public void Catch() {
        Enemies.Clear();

        foreach (Transform child in transform) {
            if (!child.gameObject.activeSelf) { continue; }
            Enemies.Add(child.GetComponent<Enemy>());
        }

        Enemies = InsertionSort(Enemies);
    }

    public void Do() {
        int esp = 0;
        Attacking.Clear();
        foreach (Enemy raged in Enemies) {
            if (esp <= 1) {
                raged.Attacking = true;
                Attacking.Add(raged);
                esp++;
            } else {
                raged.Attacking = false;
            }
        }
    }

    static List<Enemy> InsertionSort(List<Enemy> input) {
        for (int i = 0; i < input.Count - 1; i++) {
            for (int j = i + 1; j > 0; j--) {
                if (Mathf.Abs(input[j - 1].TargetDistance.magnitude) <= Mathf.Abs(input[j].TargetDistance.magnitude)) { continue; }
                Enemy temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;

            }
        }
        return input;
    }
}