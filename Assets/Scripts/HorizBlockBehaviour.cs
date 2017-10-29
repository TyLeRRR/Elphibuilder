using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizBlockBehaviour : MonoBehaviour
{
    private float timeLeftToStart = 1.5f;
    private float nextStepTimestamp;
    public bool isCollided;

    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        nextStepTimestamp = Time.realtimeSinceStartup + 0.5f;
    }

    void Update()
    {
        timeLeftToStart -= Time.deltaTime;

        if (timeLeftToStart < 0)
        {
            gameObject.GetComponent<Renderer>().enabled = true;

            //move the block every 0.5sec on 0.5 down
            if (nextStepTimestamp <= Time.realtimeSinceStartup)
            {
                transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
                nextStepTimestamp = Time.realtimeSinceStartup + 0.5f;
                
                if(IsCollided)
                {
                    transform.Translate(0,0,0);
                }
            }
        }
    }

    public bool IsCollided
    {
        get { return isCollided; }
        set { isCollided = value; }
    }
}