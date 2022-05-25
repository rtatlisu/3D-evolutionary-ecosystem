using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private bool jumping=false;
    Rigidbody rb;
    Vector3 targetAngle;
    Vector3 startPosition;
    Vector3 endPosition;
    float duration = 3f;
    float elapsedTime;
    public bool move=true;
    private float  moveTime;
    public float moveSpeed=0.6f;
    public float jumpArc=3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition=transform.position;
        endPosition=startPosition+new Vector3(10,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(move) {
            AnimateMovement();
        }
    }

   void AnimateMovement() {
         moveTime = Mathf.Min(1, moveTime+Time.deltaTime*moveSpeed);
        float height = (1-4*(moveTime-0.5f)*(moveTime-0.5f))*jumpArc;
Debug.Log(moveTime);
        transform.position=Vector3.Lerp(startPosition, endPosition, moveTime) +Vector3.up*height;

        if(moveTime>=1) {
            move=false;
            moveTime=0;
        }
   }


   

}
