using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData {
    public Vector3 playerPos; //The variable for the players current position
    public Vector3 playerDir; //The variable for the players current direction
    public Vector3 enemyPos; //The variable for the enemies current position
    public float enemyRange; //The variable for the enemies spawn range
    public int enemyNum; //The variable for the number of enemies spawned
}

public class Text : MonoBehaviour
{
    private Emene current = null; //The variable for the current enemy being attacked
    
    public LevelData[] levels; //The array for the level data
    public int level = 0; //The variable for the current level
    private int newSpawn = 0; //The variable for the number of enemies that have been spawned

    public GameObject emenePrefab; //The variable for the enemy prefab game object
    public GameObject bossPrefab; //The variable for the boss prefab game object

    private int speed = 5; //The variable for the player movement speed
    public int enemSpeed = 2; //The variable for the enemy movement speed
    private float step; //The variable for the player movement speed + game time
    private Vector3 target; //The variable for the location the player needs to move to
    private Vector3 target2; //The variable for the location the player needs to look to
    private Vector3 targetDirection; //The variable for the rotation the player needs to make
    private Vector3 newDirection; //The variable for the vector rotation
    private Quaternion finalLook; //The variable for the for the quaternion of look rotation

    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        target = transform.position; //setting the first position for the player to be at
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position; //setting pos to be the current position
        
        string currentText = Input.inputString.ToLower(); //converts typed characters to lowercase 

        Emene[] emene = GameObject.FindObjectsOfType<Emene>(); //An array to store the enemies on screen

        step = speed * Time.deltaTime; //setting the movement speed and locking it to game time

        if (emene.Length == 0)
        {
            if(level < levels.Length)
            {
                target = levels[level].playerPos; //setting the position to move to
                target2 = levels[level].playerDir; //setting the direction to look to
                targetDirection = target2 - transform.position; //math for working out the difference between current rotation and the target rotation
                targetDirection.y = 0; //locking the y axis
                finalLook = Quaternion.LookRotation(targetDirection); //converts the target to direction to a quaternion
                newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f); //setting the vector rotation

                if ((transform.position - target).sqrMagnitude > 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, step); //the code for player movement
                } else if ((transform.rotation.eulerAngles - finalLook.eulerAngles).sqrMagnitude > 1)
                {
                    transform.rotation = Quaternion.LookRotation(newDirection); //the code for player rotation
                } else
                {
                    for (newSpawn = 0; newSpawn < levels[level].enemyNum; newSpawn++)
                    {
                        Debug.Log(level);
                        //the code for setting the enemy spawn positions
                        Vector3 ep = levels[level].enemyPos; //Sets a Vector position
                        ep.x += (UnityEngine.Random.value * 2 - 1) * levels[level].enemyRange; //picks a random range from the x coordinate to allow for spawning in random locations
                        ep.z += (UnityEngine.Random.value * 2 - 1) * levels[level].enemyRange; //picks a random range from the y coordinate to allow for spawning in random locations
                        Instantiate(emenePrefab, ep, Quaternion.identity); //creates an enemy from a prefab
                    }
                    level += 1; //adds 1 to the level variable
                }
            } else if (level == 4)
            {
                //the code for spawning the boss
                enemSpeed = 1;
                Instantiate(bossPrefab, new Vector3(-25f, 5.5f, 75f), Quaternion.identity); //creates a boss from a prefab
                level += 1; //adds 1 to the level variable
            } else if (level == 5)
            {
                SceneManager.LoadScene("You Win");
            }
        } 

        while (currentText.Length > 0)
        {
            Emene bestMatch = current; //checks for the what letters match what the player types

            //code for checking and deleting typed characters
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
    //function for the gameover upon getting hit by an enemy
    void OnCollisionEnter (Collision hit)
    {
      if(hit.transform.gameObject.GetComponent<Emene>())
      {
           SceneManager.LoadScene("Game Over"); //loads the game over scene
      }
    }
}
