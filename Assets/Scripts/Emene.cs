using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class Emene : MonoBehaviour
{
    private int speed = 2; //The variable for the movement speed
    private float step; //The variable for the movement speed + game time
    private Vector3 target; //The variable for the location to move to

    public GameObject player; //The variable for the player game object
    public Text location; //The variable for the text script

    public GameObject gun;
    public AudioSource gunShot;
    public GameObject particleSystem;
    public ParticleSystem muzzleFlash;

    public GameObject emenePrefab; //The variable for the enemy prefab

    public string[] words = new string[3]{"hello", "egg", "moist"}; //the array to store all the words and enemy can have

     // Entire text to type out
    public string displayText;
    public string text => displayText.ToLower();
    // Letters typed
    private int _progress = 0;
    // Getter setter for updating visuals
    public int progress {
        get => _progress;
        set {
            int prev = _progress;
            _progress = value;
            TypeUpdate(prev);
        }
    }
    public int remainingProgress => displayText.Length - progress;
    // Time to live, if nonzero, will delete at this time
    public float ttl = 0f;
    private float startt = 0f;
    // Remaining text
    public string remainingText => text.Substring(progress);
    public string remainingDisplayText => new string(' ', progress) + displayText.Substring(progress);
    public TextMeshPro tm = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        location = player.GetComponent<Text>();
        gun = GameObject.Find("Gun");
        gunShot = gun.GetComponent<AudioSource>();
        particleSystem = GameObject.Find("MuzzleFlash");
        muzzleFlash = particleSystem.GetComponent<ParticleSystem>();
        displayText = words[UnityEngine.Random.Range(0, words.Length)];

        if (!tm) tm = GetComponent<TextMeshPro>();
        // Destroy after TTL
        if (ttl > 0f) {
            float startt = Time.time;
            Destroy(gameObject, ttl);
        }
        TypeUpdate(0);
    }

    // Returns number of matching characters at start to provided string
    public int CharsMatch(string s) {
        string rem = remainingText;
        int l = Math.Min(remainingProgress, s.Length);
        int i = 0;
        for (; i < l; i++) {
            if (rem[i] != s[i]) break;
        }
        return i;
    }

    void TypeUpdate(int prev) {
        tm.text = remainingDisplayText;
        if (remainingProgress <= 0) {
            
            int x = UnityEngine.Random.Range(-5, 5);
            int z = UnityEngine.Random.Range(0, 5);
            // Instantiate(emenePrefab, new Vector3(x, 1, z), Quaternion.identity);
            muzzleFlash.Play();
            gunShot.Play();
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        speed = location.enemSpeed;
        step = speed * Time.deltaTime;  //Sets the movement speed and locks it to the game time
        target = location.pos; //Sets the coordinates of the player as the target
        
        transform.position = Vector3.MoveTowards(transform.position, target, step); //Code for the movement

        transform.LookAt(location.transform); //Sets the direction for the enemy to look at
        Vector3 eul = transform.rotation.eulerAngles;
        eul.x = 0; //Locks the X rotation
        eul.z = 0; //Locks the Y rotation
        eul.y += 180;
        transform.eulerAngles = eul; //Sets the rotation

        if (ttl > 0f) {
            tm.color = Color.Lerp(Color.white, Color.red, (Time.time - startt) / ttl);
        }
        
    }

    
}
