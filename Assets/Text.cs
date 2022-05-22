using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text : MonoBehaviour
{
    private Emene current = null;
    
    public int level = 1;
    private int newSpawn = 0;
    
    private int x;
    private int z;
    public GameObject emenePrefab;

    private int speed = 5;
    private float step;
    private Vector3 target;
    private Vector3 target2;
    private Vector3 targetDirection;
    private Vector3 newDirection;

    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        for (int i = 0; i < 4; i++)
        {
            x = UnityEngine.Random.Range(-5, 5);
            z = UnityEngine.Random.Range(20, 30);
            Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        Debug.Log(level);
        string currentText = Input.inputString.ToLower();

        Emene[] emene = GameObject.FindObjectsOfType<Emene>();

        step = speed * Time.deltaTime;

        if (emene.Length == 0)
        {
            switch (level)
            {
                case 1:
                    target = new Vector3(0.0f, 1.2f, 26.0f);
                    target2 = new Vector3(12.5f, 1.2f, 25f);
                    targetDirection = target2 - transform.position;
                    targetDirection.y = 0;
                    newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
                    if ((transform.position - target).sqrMagnitude > 1)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target, step);

                    } else if (newDirection != targetDirection)
                    {
                        transform.rotation = Quaternion.LookRotation(newDirection);
                    } else
                    {
                        for (newSpawn = 0; newSpawn < 4; newSpawn++)
                        {
                            x = UnityEngine.Random.Range(20, 30);
                            z = UnityEngine.Random.Range(20, 30);
                            Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
                            Debug.Log("kek");
                        }
                        level += 1;
                    }

                    break;
                case 2:
                    target = new Vector3(26.0f, 1.2f, 25.0f);
                    target2 = new Vector3(25.0f, 1.2f, 50f);
                    targetDirection = target2 - transform.position;
                    targetDirection.y = 0;
                    newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
                    if ((transform.position - target).sqrMagnitude > 1)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target, step);

                    }
                    else if (newDirection != targetDirection)
                    {
                        transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                    else {
                        for (newSpawn = 0; newSpawn < 4; newSpawn++)
                        {
                            x = UnityEngine.Random.Range(20, 30);
                            z = UnityEngine.Random.Range(45, 55);
                            Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
                        } 
                    }    
                    level += 1;
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
