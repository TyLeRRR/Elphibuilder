using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour{
    public static float gridWidth = 0.63f; // 15 elements

//    public static float gridHeight = 0.36f; // 9 positions
    public static float gridHeight = 0.27f; // 

    public static ArrayList coordinates = new ArrayList{
        0.0f,
        0.045f,
        0.09f,
        0.135f,
        0.18f,
        0.225f,
        0.27f,
        0.315f,
        0.36f,
        0.405f,
        0.45f,
        0.495f,
        0.54f,
        0.585f,
        0.63f
    };

    public static Transform[,] grid = new Transform[15, 11];
    
    

    private static GameObject klaueOpened;
    private static GameObject klaueClosed;
    private static GameObject kran;

    public static int stackCounter = 1;

    public int scoreOneLine = 10000;
    public int scoreTwoLine = 30000;
    public int scoreThreeLine = 50000;
    private int numberOfRowsThisTurn = 0;

    public Text hud_score;
    public static int curr_score = 1200000;
    

    public GameObject crowd;
    private static Animator crowdAnimator;
    private static Animator roofAnimator;

    public static GameObject roof;
    public GameObject politician;

    public static bool isGameOverFail = false;
    public static bool isGameOverWin = false;

    void Start(){

        //prefill grid with fake data for GAME WIN video
//        for (int h = 0; h < 5; ++h){
//            for (int w = 0; w < 15; ++w){
////                grid[w, h] = 
//            }
//        }
        Screen.SetResolution(900,1200,false);
        
        klaueOpened = GameObject.Find("Klaue_1");
        klaueClosed = GameObject.Find("Klaue_0");
        kran = GameObject.Find("CraneTALL");
        
        roof = GameObject.Find("Roof");
        

        klaueClosed.SetActive(false);
        klaueOpened.SetActive(false);
        roof.SetActive(false);
        
        crowd = GameObject.Find("Crowd");
        crowdAnimator = crowd.GetComponent<Animator>();
        crowdAnimator.enabled = false;

        roofAnimator = roof.GetComponent<Animator>();
        roofAnimator.enabled = false;
        
        
        SpawnNextBlock();
        StartCoroutine(ExecuteAfterTime(1));
        
        politician = GameObject.Find("Politician1_0");
        politician.GetComponent<Renderer>().enabled = false;
    }

    void Update(){
        UpdateScore();
        UpdateUI();
        if(isGameOverFail) PlayFailGameOver();
    }

    public void UpdateScore(){
        if (numberOfRowsThisTurn > 0){
            if (numberOfRowsThisTurn == 1){
                OneLineFull();
            }
            else if (numberOfRowsThisTurn == 2){
                TwoLinesFull();
            }
            else if (numberOfRowsThisTurn == 3){
                ThreeLinesFull();
            }
            numberOfRowsThisTurn = 0;
        }
    }

    public void OneLineFull(){
        curr_score += scoreOneLine;
    }

    public void TwoLinesFull(){
        curr_score += scoreTwoLine;
    }

    public void ThreeLinesFull(){
        curr_score += scoreThreeLine;
    }

    public void UpdateUI(){
        hud_score.text = curr_score.ToString();
    }

    public void UpdateGrid(Block block){
        for (int y = 0; y < 7; ++y){
            for (int x = 0; x < 15; ++x){
                if (grid[x, y] != null){
                    if (grid[x, y].parent == block.transform){
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform b in block.transform){
            Vector2 pos = Round(b.position);

            if (pos.y < gridHeight){
                int tempX = coordinates.IndexOf(pos.x);
                int tempY = coordinates.IndexOf(pos.y);
                grid[tempX, tempY] = b;
                IsFullRowAt(tempY);
                
                if(IsLastRowFull()){
                    PlayWinGameOver();
                }
            }
        }
    }
    
    public void PlayWinGameOver(){
//        klaueOpened.SetActive(false);
        Destroy(klaueClosed);
        Destroy(klaueOpened);
//        klaueClosed.SetActive(false);
        kran.SetActive(false);

        StartCoroutine(RoofThanCrowd(4));
        
    }

//    private void SpawnRoof(){
//        String roof = "Prefabs/Roof";
//        
//        Instantiate(Resources.Load(roof, typeof(GameObject)),
//            new Vector2(0.315f, 0.63f), Quaternion.identity);
//    }
    
    public void PlayFailGameOver(){
        politician.GetComponent<Renderer>().enabled = true;
    }

    public bool IsFullRowAt(int y){
        for (int x = 0; x < 15; ++x){
            if (grid[x, y] == null){
                return false;
            }
        }

        //Since we found a full row, we increment the full row variable.
        numberOfRowsThisTurn++;
        return true;
    }
    
    public static bool HasIsland(){
        for (int w = 1; w < 15; w++){
            for (int h = 1; h < 7; h++){
                if (grid[w, h] == null){
                    if(grid[w-1,h] !=null && grid[w+1,h] != null && grid[w, h+1] != null && grid[w, h-1] != null){
                        return true;
                    }
//                    if(grid[w-1,h] !=null && grid[w+1,h] != null && grid[w, h+1] != null && grid[w, h-1] == null && grid[w,h-2] != null){
//                        return true;
//                    }
//                    if(grid[w-1,h] !=null && grid[w+1,h] != null && grid[w, h+1] == null && grid[w,h+2] != null && grid[w, h-1] != null){
//                        return true;
//                    }
                }
            }
        }

        return false;
    }
    
    public bool IsLastRowFull(){
        for (int x = 0; x < 15; ++x){
            if (grid[x, 5] == null){
                return false;
            }
        }

        return true;
    }

    public Transform GetTransformAtGridPosition(Vector2 pos){
        if (pos.y > gridHeight - 0.045f){
            return null;
        }
        else{
            int tempX = coordinates.IndexOf(pos.x);
            int tempY = coordinates.IndexOf(pos.y);
            return grid[tempX, tempY];
        }
    }

    public bool CheckIsInsideGrid(Vector2 pos){
        return (pos.x >= 0f && pos.x < gridWidth + 0.025f && pos.y >= 0f);
    }

    public Vector2 Round(Vector2 pos){
        float x = (float) Math.Round((double) pos.x, 3);
        float y = (float) Math.Round((double) pos.y, 3);

        return new Vector2(x, y);
    }

    public void SpawnNextBlock(){
        swapKlaueToOpen();
        String currBlock = GetRandomBlock();
        if (currBlock == "Prefabs/Horiz2"){
            Instantiate(Resources.Load(currBlock, typeof(GameObject)),
                new Vector2(0.315f, 0.585f), Quaternion.identity);
        }
        else if (currBlock == "Prefabs/Vertic2"){
            Instantiate(Resources.Load(currBlock, typeof(GameObject)),
                new Vector2(0.36f, 0.495f), Quaternion.identity);
        }
        else if (currBlock == "Prefabs/4x4Block"){
            Instantiate(Resources.Load(currBlock, typeof(GameObject)),
                new Vector2(0.315f, 0.54f), Quaternion.identity);
        }
    }
//    public void SpawnNextHORIZBlock(){
//        swapKlaueToOpen();
////        String currBlock = GetRandomBlock();
////        if (currBlock == "Prefabs/Horiz2"){
//            Instantiate(Resources.Load("Prefabs/Horiz2", typeof(GameObject)),
//                new Vector2(0.315f, 0.585f), Quaternion.identity);
////        }
//    }
//    public void SpawnNextVERTICBlock(){
//        swapKlaueToOpen();
//            Instantiate(Resources.Load("Prefabs/Horiz2", typeof(GameObject)),
//                new Vector2(0.315f, 0.585f), Quaternion.identity);
//        }
//        else if (currBlock == "Prefabs/Vertic2"){
//            Instantiate(Resources.Load(currBlock, typeof(GameObject)),
//                new Vector2(0.36f, 0.495f), Quaternion.identity);
//        }
//        else if (currBlock == "Prefabs/4x4Block"){
//            Instantiate(Resources.Load(currBlock, typeof(GameObject)),
//                new Vector2(0.315f, 0.54f), Quaternion.identity);
//        }
//    }


    string GetRandomBlock(){
        int rand = Random.Range(1, 4);
        string randomBlockName = "Prefabs/Horiz2";

        switch (rand){
            case 1:
                randomBlockName = "Prefabs/Horiz2";
                break;
            case 2:
                randomBlockName = "Prefabs/Vertic2";
                break;
            case 3:
                randomBlockName = "Prefabs/4x4Block";
                break;
        }

        return randomBlockName;
    }

    public static void swapKlaueToClose(){
        klaueOpened.SetActive(false);
        klaueClosed.SetActive(true);
    }

    public static void swapKlaueToOpen(){
        klaueClosed.SetActive(false);
        klaueOpened.SetActive(true);
    }

    IEnumerator ExecuteAfterTime(float time){
        yield return new WaitForSeconds(time);
        swapKlaueToClose();
    }

    IEnumerator RoofThanCrowd(float time){
        roof.SetActive(true);
        roofAnimator.enabled = true;
        yield return new WaitForSeconds(time);
        crowdAnimator.enabled = true;
    }

    public static int StackCounter{
        get{ return stackCounter; }
        set{ stackCounter = value; }
    }
}