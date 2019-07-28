using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour{
    public float speed = 0.5f;

    void Start(){
        transform.position = new Vector3(transform.position.x, 0.585f, transform.position.z);
    }

    void Update(){
        transform.Translate(0, -speed * Time.deltaTime, 0);

        //if the coinStack is 0 -> destroy at -0.5f hight
        if (gameObject.transform.position.y <= -0.5f){
            Destroy(gameObject);
            SpawnNewStackCoin();

            Game.StackCounter++;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "coinStack"){
            Destroy(gameObject);
            SpawnNewStackCoin();
            Game.StackCounter++;
        }
    }

    void SpawnNewStackCoin(){
        String coin = "Prefabs/CoinStack" + Game.StackCounter;
        if (Game.StackCounter > 1){
            String tempName = "CoinStack" + (Game.StackCounter - 1);
            Destroy(GameObject.Find(tempName+ "(Clone)"));
        }
        Instantiate(Resources.Load(coin, typeof(GameObject)), new Vector2(0.93f, -0.44f), Quaternion.identity);
    }
}