using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool CanMove = false;
    public GameObject pd;

    private Vector3 center = new Vector3(Screen.width * .5f, Screen.height * .5f, 0);
    private Vector3 Distance => center - pd.transform.position;

    private void FixedUpdate()
    {
        if (!CanMove) { return; }
        if (Distance.sqrMagnitude < Mathf.Pow(2, 2)) { CanMove = false; }

        Vector3 temp = pd.transform.position;
        temp.x += (Distance.x / Mathf.Abs(Distance.x)) * 2;

        pd.transform.position = temp;
    }

    public void Moves()
    {
        CanMove = true;
    }
}
