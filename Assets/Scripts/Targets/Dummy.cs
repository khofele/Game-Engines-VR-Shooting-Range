using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float health = 60f;
    [SerializeField] private static int pointsPerTarget = 5; //points the player gets after "killing" target
    private bool down = false; //true if target has fallen
    private float currentHealth = 0f;

    private WaitForSeconds delay; //delay until shield is put up again

    [SerializeReference] private GameManager gm = null;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        delay = new WaitForSeconds(12f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!down && currentHealth <= 0)
        {
            down = true; //target has fallen

            //trigger animation
            gameObject.GetComponent<Animator>().SetTrigger("dead");

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
            gameObject.GetComponent<Animator>().SetTrigger("pushed");
        }
    }

    //reset down + health (target is put up and can be shot down again)
    IEnumerator ResetFall()
    {
        yield return delay;
        //trigger idle animation
        gameObject.GetComponent<Animator>().ResetTrigger("dead");
        gameObject.GetComponent<Animator>().SetTrigger("idle");
        //reset values
        down = false;
        currentHealth = health;
    }
}
