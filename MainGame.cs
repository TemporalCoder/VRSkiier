using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainGame : MonoBehaviour
{
    [Header("Tracking Points")]
    public GameObject Head;

    [Header("UI")]
    public TMP_Text dataOutTxt;    
    public TMP_Text timerTB;        //textbox on canvas

    //Accessed from Collision Control where are the hands? - Better method?? 
    static public int LeftHand;  //-1 Left, 0 Center , 1 Right
    static public int RightHand;

    [Header("CheckPoints")]
    public GameObject flags;
    List<GameObject> checkPoints = new List<GameObject>();
    public int numOfCheckPoints = 10;
    public int spaceBetween = 20;   //distance between flags/checkpoints
    public int range = 5;           //random range 

    [Header("Misc Variables")]
    public string levelToLoad;
    public float timer = 30;

    [Header("Data and Debug")]
    static public float standingHeight;
    public float maxSpeed;
    public float currentSpeed;

    static public float fVel = 0.0f; //velocity
    public float maxVel = 10; //max velocity
    public float friction = 1.0f; //slowdown factor

    static int gameState = 0; // 0-start screen, 1-main game , 2 - game over

    // Start is called before the first frame update
    void Start()
    {
        if (gameState == 1)
        {
            initCheckPoints();
            fVel = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Start Screen
        if (gameState == 0)
        {
            UIStuff();
            standingHeight = Head.transform.position.y; //get eyeline height from VR Headset. 
            
            if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKey(KeyCode.Space))  //Quest A Button
            {
                standingHeight = Head.transform.position.y; //replace with initise function.         

                gameState = 1;

                //ToDo... play whooshy sound and transition
                SceneManager.LoadScene(levelToLoad);
            }
        }
        
        //Main Game
        if (gameState==1)
        {

            timerTB.text = "Time: " + timer.ToString("0.0");
           // timerTB.text += "\n SH:" + standingHeight.ToString() 
             //               + "\nH: " + Head.transform.position.y;

            timer -= Time.deltaTime;

            if (timer < 0)  //Game over
            {
                //end of game goto main/end screen. 
                //SceneManager.LoadScene(levelToLoad);
            }

            //Update CheckPoints
            moveCheckPoints();

            //VR Controller
            if (LeftHand == -1 && RightHand == -1) { moveLeft(); }
            if (LeftHand == 1 && RightHand == 1) { moveRight(); }
                        
            if (Head.transform.position.y < standingHeight - 0.15f) //~15cm (1/2 a foot ~6inch)
            {
                if (fVel < maxVel)
                {
                    fVel += 30 * Time.deltaTime;
                }
            }

            //for testing only
            //keys for testing
            if (Input.GetKey(KeyCode.LeftArrow)) { moveLeft(); }
            if (Input.GetKey(KeyCode.RightArrow)) { moveRight(); }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (fVel < maxVel)
                {
                    fVel += 30 * Time.deltaTime;
                }
            }


            //slow down 
            if (fVel > 0.0f)
            {
                fVel -= friction * Time.deltaTime;
                if (fVel < 0.0f) { fVel = 0.0f; }
            }

            //Reset or if fell off the land 
            if (transform.position.y < -5 || Input.GetKey(KeyCode.R))
            {               
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }

    }


    void UIStuff()
    {
        dataOutTxt.text = "Data:\nS.H. " + standingHeight.ToString("0.00");
        dataOutTxt.text = "H.H. " + Head.transform.position.y.ToString("0.00");
       // dataOutTxt.text += "\nSpeed: " + currentSpeed.ToString("0.0");

    }

    void initCheckPoints()
    {
        for (int i = 0; i < numOfCheckPoints; i++)
        {
            int x = Random.Range(-range, range);
            GameObject CP = Instantiate(flags, new Vector3(x, 0, i * spaceBetween), Quaternion.identity);
            checkPoints.Add(CP);
        }
    }

    void moveCheckPoints()
    {
        //move the flags - based on player speed.
        for (int i = 0; i < numOfCheckPoints; i++)
        {
            Vector3 temp = checkPoints[i].transform.position;
            temp.z -= fVel * Time.deltaTime;
            if (temp.z < -10)
            {
                temp.z = numOfCheckPoints * spaceBetween;
                temp.x = Random.Range(-range, range);
                if (i == 0 && range <= 10) { range += 1; }
            }
            checkPoints[i].transform.position = temp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            timer += 2;
        }
    }
    void moveRight()
    {
        Vector3 temp = transform.position;
        temp.x += 1 * Time.deltaTime;
        transform.position = temp;
    }

    void moveLeft()
    {
        Vector3 temp = transform.position;
        temp.x -= 1 * Time.deltaTime;
        transform.position = temp;
    }
}
