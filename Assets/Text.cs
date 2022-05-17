using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text : MonoBehaviour
{
    private Emene current = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string currentText = Input.inputString.ToLower();

        Emene[] emene = GameObject.FindObjectsOfType<Emene>();

        if (emene.Length == 0)
        {
            return;
        }
        
        while (currentText.Length > 0)
        {
            Emene bestMatch = current;

            int matchNum = bestMatch ? bestMatch.CharsMatch(currentText) : 0;
            foreach (Emene t in emene)
            {
                if (!t) 
                {
                    continue;
                }
                int i = t.CharsMatch(currentText);
                if (i > matchNum)
                {
                    matchNum = i;
                    bestMatch = t;
                }
                
            }
            if (bestMatch && matchNum > 0) {
                // Save it as the previously typed enemy
                current = bestMatch;
                // Tell it some characters were typed and remove some input
                bestMatch.progress += matchNum;
                currentText = currentText.Substring(matchNum);
            } else {
                // No matches, drop that character
                currentText = currentText.Substring(1);
            }
        }  
    }
     
    void OnCollisionEnter (Collision hit)
    {
      if(hit.transform.gameObject.GetComponent<Emene>())
      {
           SceneManager.LoadScene("Game Over");
      }
    }
}
