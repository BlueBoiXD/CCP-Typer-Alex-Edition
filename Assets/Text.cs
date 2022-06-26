using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData {
    public Vector3 playerPos;
    public Vector3 playerDir;
    public Vector3 enemyPos;
    public float enemyRange;
    public int enemyNum;
}

public class Text : MonoBehaviour
{
    private Emene current = null;
    
    public LevelData[] levels;
    public int level = 0;
    private int newSpawn = 0;

    private int x;
    private int z;
    public GameObject emenePrefab;
    public GameObject bossPrefab;

    private int speed = 5;
    private float step;
    private Vector3 target;
    private Vector3 target2;
    private Vector3 targetDirection;
    private Vector3 newDirection;
    private Quaternion finalLook;

    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        
        string currentText = Input.inputString.ToLower();

        Emene[] emene = GameObject.FindObjectsOfType<Emene>();

        step = speed * Time.deltaTime;

        if (emene.Length == 0)
        {
            if(level < levels.Length)
            {
                target = levels[level].playerPos;
                target2 = levels[level].playerDir;
                targetDirection = target2 - transform.position;
                targetDirection.y = 0;
                finalLook = Quaternion.LookRotation(targetDirection);
                newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);

                if ((transform.position - target).sqrMagnitude > 1)
                {

                    transform.position = Vector3.MoveTowards(transform.position, target, step);
                    Debug.Log("kek");

                } else if ((transform.rotation.eulerAngles - finalLook.eulerAngles).sqrMagnitude > 1)
                {
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    Debug.Log("hek");
                } else
                {
                    for (newSpawn = 0; newSpawn < levels[level].enemyNum; newSpawn++)
                    {
                        Vector3 ep = levels[level].enemyPos;
                        ep.x += (UnityEngine.Random.value * 2 - 1) * levels[level].enemyRange;
                        ep.z += (UnityEngine.Random.value * 2 - 1) * levels[level].enemyRange;
                        Instantiate(emenePrefab, ep, Quaternion.identity);
                    }
                    level += 1;
                }
            } else
            {
                if (level <= 4)
                {
                    speed = 2;
                    Instantiate(bossPrefab, new Vector3(-25f, 5.5f, 75f), Quaternion.identity);
                    level += 1;
                }
            }
            
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
