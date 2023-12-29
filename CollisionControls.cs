using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControls : MonoBehaviour
{
    public string hand; //left or right

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Left" && hand == "Left") { MainGame.LeftHand = -1; }
        if (other.gameObject.tag == "Left" && hand == "Right") { MainGame.LeftHand = 1; }

        if (other.gameObject.tag == "Right" && hand == "Left") { MainGame.RightHand = -1; }
        if (other.gameObject.tag == "Right" && hand == "Right") { MainGame.RightHand = 1; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Left" ) { MainGame.LeftHand = 0; }
        if (other.gameObject.tag == "Right" ) { MainGame.RightHand = 0; }
    }
}
