using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour{
    private float fall = 0;
    public float fallSpeed = 1f;

    public int individualScore = 100000;
    private float individualScoreTime;

    void Start(){
    }

    void Update(){
        CheckUserInput();
        UpdateIndividualScore();
    }

    void UpdateIndividualScore(){
        if (individualScoreTime < 1){
            individualScoreTime += Time.deltaTime;
        }
        else{
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    void CheckUserInput(){
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            transform.position += new Vector3(0.0450f, 0, 0);

            if (CheckIsValidPosition()){
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(-0.0450f, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            transform.position += new Vector3(-0.0450f, 0, 0);


            if (CheckIsValidPosition()){
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(0.0450f, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed){
            //round the values taken from transform.position
            var temp = transform.position;
            temp.x = (float) Math.Round((double) temp.x, 3);
            temp.y = (float) Math.Round((double) temp.y, 3) + (float) Math.Round(-0.0450f, 3);
            transform.position = temp;

            if (CheckIsValidPosition()){
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(0, 0.0450f, 0);
                enabled = false;
                StartCoroutine(SpawnCoinWaitSpawnBlock(1));
                StartCoroutine(ExecuteAfterTime(2));
                Game.curr_score -= individualScore;
            }

            fall = Time.time;
        }
    }


    bool CheckIsValidPosition(){
        foreach (Transform block in transform){
            Vector2 pos = FindObjectOfType<Game>().Round(block.position);

            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false){
                return false;
            }

            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null &&
                FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform){
                return false;
            }
        }

        return true;
    }

    IEnumerator ExecuteAfterTime(float time){
        yield return new WaitForSeconds(time);
        Game.swapKlaueToClose();
    }

    public void SpawnNewCoin(){
        String coin = "Prefabs/CoinFall_0";
        Instantiate(Resources.Load(coin, typeof(GameObject)), new Vector2(0.93f, 0.585f), Quaternion.identity);
    }

    IEnumerator SpawnCoinWaitSpawnBlock(float time){
        SpawnNewCoin();
        Game.curr_score -= 30000;
        yield return new WaitForSecondsRealtime(time);
        if (Game.HasIsland()){
            Game.isGameOverFail = true;
        }
        if (!Game.isGameOverFail && !Game.isGameOverWin){
            FindObjectOfType<Game>().SpawnNextBlock();
        }
    }
}