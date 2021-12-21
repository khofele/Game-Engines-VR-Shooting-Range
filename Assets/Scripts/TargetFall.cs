using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFall : MonoBehaviour
{
    public float health = 50; 
    float yTransform = -2.48f; //how position.y is transformend when target falls down
    bool down = false; //true if target has fallen

    private WaitForSeconds delay; //delay until shield is put up again

    // Start is called before the first frame update
    void Start()
    {
        delay = new WaitForSeconds(20f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!down && health <= 0)
        {
            down = true; //target has fallen

            //trigger animation
            gameObject.GetComponent<Animator>().SetTrigger("Fall");
            //move y-Position -2.48 down 
            transform.position = new Vector3(transform.position.x, transform.position.y - yTransform, transform.position.z);

            //reset after some time 
            StartCoroutine(ResetFall()); //reset down after time (delay) - shield can be hitted again
        }

    }

    //method to call with weapon - target has been hit
    public void Hit()
    {
        health = health - 5;
    }

    //reset down + health (target is put up and can be shot down again)
    IEnumerator ResetFall()
    {
        yield return delay;
        //trigger idle animation
        gameObject.GetComponent<Animator>().ResetTrigger("Fall");
        gameObject.GetComponent<Animator>().SetTrigger("Idle");
        //reset values
        down = false;
        health = 50;
    }
}
