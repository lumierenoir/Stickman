using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanController : MonoBehaviour
{

    public _Muscle[] muscles;

    public bool Right;
    public bool Left;

    public Rigidbody2D rbRIGHT;
    public Rigidbody2D rbLEFT;

    public Rigidbody2D rbHead;
    public Rigidbody2D rbHips;

    public Rigidbody2D rbUpperLegRight;
    public Rigidbody2D rbUpperLegLeft;

    public Vector2 WalkRightVector;
    public Vector2 WalkLeftVector;

    [Tooltip("Fraction of walk force applied to head and hips (e.g. 0.3)")]
    public float upperBodyForceMultiplier = 0.3f;

    [Tooltip("Upward force applied to the upper leg when lifting it during a step")]
    public float legLiftForce = 3f;

    private float MoveDelayPointer;
    public float MoveDelay;


    // Update is called once per frame
    private void Update()
    {
        foreach (_Muscle muscle in muscles)
        {
            muscle.ActivateMuscle();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Right = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Left = true;
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            Left = false;
            Right = false;
        }


        while (Right == true && Left == false && Time.time > MoveDelayPointer)
        {
            Invoke("Step1Right", 0f);
            Invoke("Step2Right", 0.085f);
            MoveDelayPointer = Time.time + MoveDelay;
        }

        while (Left == true && Right == false && Time.time > MoveDelayPointer)
        {
            Invoke("Step1Left", 0f);
            Invoke("Step2Left", 0.085f);
            MoveDelayPointer = Time.time + MoveDelay;
        }
    }

    private void PushUpperBody(Vector2 direction)
    {
        Vector2 upperBodyForce = direction * upperBodyForceMultiplier;
        rbHead.AddForce(upperBodyForce, ForceMode2D.Impulse);
        rbHips.AddForce(upperBodyForce, ForceMode2D.Impulse);
    }

    private void LiftLeg(Rigidbody2D upperLeg)
    {
        upperLeg.AddForce(Vector2.up * legLiftForce, ForceMode2D.Impulse);
    }

    public void Step1Right()
    {
        LiftLeg(rbUpperLegRight);
        rbRIGHT.AddForce(WalkRightVector, ForceMode2D.Impulse);
        rbLEFT.AddForce(WalkRightVector * -0.5f, ForceMode2D.Impulse);
        PushUpperBody(WalkRightVector);
    }

    public void Step2Right()
    {
        LiftLeg(rbUpperLegLeft);
        rbLEFT.AddForce(WalkRightVector, ForceMode2D.Impulse);
        rbRIGHT.AddForce(WalkRightVector * -0.5f, ForceMode2D.Impulse);
        PushUpperBody(WalkRightVector);
    }

    public void Step1Left()
    {
        LiftLeg(rbUpperLegLeft);
        rbRIGHT.AddForce(WalkLeftVector, ForceMode2D.Impulse);
        rbLEFT.AddForce(WalkRightVector * -0.5f, ForceMode2D.Impulse);
        PushUpperBody(WalkLeftVector);
    }

    public void Step2Left()
    {
        LiftLeg(rbUpperLegRight);
        rbLEFT.AddForce(WalkLeftVector, ForceMode2D.Impulse);
        rbRIGHT.AddForce(WalkLeftVector * -0.5f, ForceMode2D.Impulse);
        PushUpperBody(WalkLeftVector);
    }
}
[System.Serializable]
public class _Muscle
{
    public Rigidbody2D bone;
    public float restRotation;
    public float force;

    public void ActivateMuscle()
    {
        bone.MoveRotation(Mathf.LerpAngle(bone.rotation, restRotation, force * Time.deltaTime));
    }
}