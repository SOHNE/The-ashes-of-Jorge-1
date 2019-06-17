using UnityEngine;

public class DevBuild : MonoBehaviour {

    [SerializeField] private KeyCode[] cheatCode;
    [SerializeField] private int index = 0;

    void Start() {
        // Code is "idkfa", user needs to input this in the right order
        cheatCode = new KeyCode[] {
            KeyCode.UpArrow, KeyCode.UpArrow,
            KeyCode.DownArrow, KeyCode.DownArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow,
            KeyCode.B, KeyCode.A
        };

    }

    void Update() {
        if (!Input.anyKeyDown) { return; }

        // Check if the next key in the code is pressed
        if (Input.GetKeyDown(cheatCode[index])) {
            // Add 1 to index to check the next key in the code
            index++;
        }
        // Wrong key entered, we reset code typing
        else {
            index = 0;
        }

        // If index reaches the length of the cheatCode string, 
        // the entire code was correctly entered
        if (index == cheatCode.Length) {
            string play = @"Music/BostaCode/{0}";
            play = string.Format(play, Random.Range(0, 3));
            FindObjectOfType<Maestria>().NewSoundtrack(play);

            index = 0;
        }
    }

}