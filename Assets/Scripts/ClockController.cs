using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameManager thisGameManager;
    public AudioSource clockTick;
    public RectTransform clockPointer;
    public float lowestClockTime = 0.15f;
    public float zRotationAtDefault;
    public float timePerRotation;
    public bool rotate;

    private float timeSinceLastRotate = 0f;
    public float percentageOfRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        clockPointer.rotation = Quaternion.Euler(0f, 0f, zRotationAtDefault);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(timePerRotation > 0f);

        if(rotate)
        {
            //update percentage
            percentageOfRotation = timeSinceLastRotate / timePerRotation;

            //set rotation
            clockPointer.rotation = Quaternion.Euler(0f, 0f, (-percentageOfRotation * 360f) + zRotationAtDefault);

            //update time
            timeSinceLastRotate = timeSinceLastRotate + Time.deltaTime;

            //if has reached end of turn
            if (timeSinceLastRotate > timePerRotation)
            {
                timeSinceLastRotate = 0f;
                thisGameManager.OnClockTick();
                clockTick.Play();
            }
        }
    }

    public void ChangeTimerPerRotation(float change)
    {
        timePerRotation += change;
        if (timePerRotation < lowestClockTime) timePerRotation = lowestClockTime;
    }
}
