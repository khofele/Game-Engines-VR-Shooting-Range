using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private static int pointsPerTarget = 2; //points the player gets after hitting the target
    [SerializeReference] private GameManager gm = null;


    //method to call with weapon - target has been hit
    public void TargetHit()
    {
        //get points for target hit
        gm.AddPoints(pointsPerTarget);
    }


}
