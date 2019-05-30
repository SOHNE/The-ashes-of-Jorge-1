using System.Collections.Generic;
using UnityEngine;

public class OthersManager : MonoBehaviour {
    public List<Enemy> Enemies;
    public int Attackers;

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
        Attackers = 0;

        foreach (Enemy raged in Enemies) {

            if (Attackers <= 1) {
                raged.Mode = 1;
                Attackers++;
            } else if (Attackers >= 4) {
                raged.Mode = 0;
            }
        }
    }

    private static List<Enemy> InsertionSort(List<Enemy> input) {
        for (int i = 0; i < input.Count - 1; i++) {
            for (int j = i + 1; j > 0; j--) {
                if (Mathf.Abs(input[j - 1].PlayerDistance.magnitude) <= Mathf.Abs(input[j].PlayerDistance.magnitude)) { continue; }

                Enemy temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;
            }
        }
        return input;
    }
}