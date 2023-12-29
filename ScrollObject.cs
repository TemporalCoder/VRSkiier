using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{

    public int numberOfTiles = 3;
    public int tileSize = 10;   //default size of plane = 10 units. 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = transform.position;          //get current position
        temp.z -= MainGame.fVel * Time.deltaTime;    //update based on movment of skiier - move the world not the player
        if (transform.position.z <= -transform.localScale.z*10) 
        {   
            temp.z = (transform.localScale.z * tileSize*(numberOfTiles-1))-1; //reset tile for infinite scrolling  
        }
        transform.position = temp;  //apply the changes
    }
}
