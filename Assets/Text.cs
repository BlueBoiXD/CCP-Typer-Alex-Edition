using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text : MonoBehaviour
{
    private Emene current = null;
    
    public int level = 1;
    
    private int x;
    private int z;
    public GameObject emenePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            x = UnityEngine.Random.Range(-5, 5);
            z = UnityEngine.Random.Range(0, 5);
            Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(level);
        string currentText = Input.inputString.ToLower();

        Emene[] emene = GameObject.FindObjectsOfType<Emene>();

        if (emene.Length == 0)
        {
            level += 1;
            switch (level)
            {
                case 2:
                    for (int newSpawn = 0;  newSpawn < 4; newSpawn ++)
                    {  
                        x = UnityEngine.Random.Range(-5, 5);
                        z = UnityEngine.Random.Range(10, 15);
                        Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
                    }
                    break;
                case 3:
                    for (int newSpawn = 0; newSpawn < 4; newSpawn++)
                    {
                        x = UnityEngine.Random.Range(-5, 5);
                        z = UnityEngine.Random.Range(20, 25);
                        Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
                    }
                    break;
                default:
                    Debug.Log("kek");
                    break;
            }
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
