using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManagerScript : MonoBehaviour{
    //Set this variable to your Cloud Prefab through the Inspector
    public GameObject cloudPrefabLong;

    public GameObject cloudPrefabShort;

    //Set this variable to how often you want the Cloud Manager to make clouds in seconds.
    //For Example, I have this set to 2
    public float delay = 10;

    //If you ever need the clouds to stop spawning, set this variable to false, by doing: CloudManagerScript.spawnClouds = false;
    public static bool spawnClouds = true;

    // Use this for initialization
    void Start(){
        //Begin SpawnClouds Coroutine
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds(){
        //This will always run
        while (true){
            //Only spawn clouds if the boolean spawnClouds is true
            while (spawnClouds){
                //Instantiate Cloud Prefab and then wait for specified delay, and then repeat
                int rand = Random.Range(1, 3);
                if (rand == 1){
                    Instantiate(cloudPrefabLong);
                }
                else if (rand == 2){
                    Instantiate(cloudPrefabShort);
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }
}