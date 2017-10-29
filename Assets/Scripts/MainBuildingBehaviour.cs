using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingBehaviour : MonoBehaviour
{
//	private GameObject horizBlock;
    public HorizBlockBehaviour horizBlock;

    void Start()
    {
        GameObject gameObject = GameObject.Find("BlockHorizontal");
    }

    void Update()
    {
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlockHorizontal"))
        {
            horizBlock = gameObject.GetComponent<HorizBlockBehaviour>();
            horizBlock.IsCollided = true;
        }
    }
}