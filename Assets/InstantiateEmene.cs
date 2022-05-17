using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEmene : MonoBehaviour
{

    public GameObject emenePrefab;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        for (int i=0; i < 4; i++) {
        int x = UnityEngine.Random.Range(-5, 5);
        int z = UnityEngine.Random.Range(0, 5);

        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
