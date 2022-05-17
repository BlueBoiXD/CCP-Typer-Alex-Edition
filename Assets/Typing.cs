using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Typing : MonoBehaviour
{
    public Text gt;

    public GameObject emene;
    public Emene es;

    void Start()
    {
        gt = GetComponent<Text>();
        emene = GameObject.Find("Emene");
        es = emene.GetComponent<Emene>();
    }

    void Update()
    {
        
    }
}
