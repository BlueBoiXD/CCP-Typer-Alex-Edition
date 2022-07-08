using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("space")) {
			// Code for starting the game so it can be played
			SceneManager.LoadScene("Gameplay");
		}
    }
}
