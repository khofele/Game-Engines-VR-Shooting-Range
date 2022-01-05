using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFall : MonoBehaviour
{
    [SerializeField] private float health = 60f; //set max health
    [SerializeField] private static int pointsPerTarget = 5; //points the player gets after "killing" target
    private float currentHealth = 0f; //current health
    private bool down = false; //true if target has fallen

    private WaitForSeconds delay; //delay until shield is put up again

    [SerializeReference] private GameManager gm = null;

    // Start is called before the first frame update
    void Start()
    {
        delay = new WaitForSeconds(12f);
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(!down && currentHealth <= 0)
        {
            down = true; //target has fallen

            //trigger animation
            gameObject.GetComponent<Animator>().SetTrigger("Fall");
            //set target down
            StartCoroutine(Fall());

            //get points for target "death"
            gm.AddPoints(pointsPerTarget);

            //reset after some time 
            StartCoroutine(ResetFall()); //reset down after time (delay) - shield can be hitted again
        }

    }

    //method to call with weapon - target has been hit
    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }

    //reset down + health (target is put up and can be shot down again)
    IEnumerator ResetFall()
    {
        yield return delay;
        //trigger idle animation
        gameObject.GetComponent<Animator>().ResetTrigger("Fall");
        gameObject.GetComponent<Animator>().SetTrigger("Idle");
        //set target back up
        transform.position = new Vector3(transform.position.x, 9.16f, transform.position.z); //shields on target hill = height standing 
        //reset values
        down = false;
        currentHealth = health;
    }

    IEnumerator Fall()
    {
        //transform.position = new Vector3(transform.position.x, 6.65f, transform.position.z); //shields on target hill = height fallen
        float inTime = 0.83f;

        Vector3 fromPos = transform.position;
        Vector3 Endpos = new Vector3(transform.position.x, 6.65f, transform.position.z); //shields on target hill = height fallen

        for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {

            transform.position = Vector3.Lerp(fromPos, Endpos, t);

            yield return null;
        }
    }
}
