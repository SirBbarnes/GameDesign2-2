using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    bool canShoot = true;
    float shootTimer = 5.0f;

    // Use this for initialization
	void Start ()
    {
        StartCoroutine(demo());
        StartCoroutine(slowUpdate());
        //Execution will continue
        Debug.Log("Inside Start");
	}

    public IEnumerator slowUpdate()
    {
        while(true)
        {
            if(Input.GetAxisRaw("Jump")>0 && canShoot)
            {
                canShoot = false;
                Debug.Log("shooting");


                StartCoroutine(addTwoInts(10,15));

                //object[] parms = new object[2] { 10, 15 };
                //StartCoroutine("addTwoInts",parms);
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    public IEnumerator addTwoInts(int a, int b)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("A+B=" + (a + b));
    }


    public IEnumerator demo()
    {
        //Do some code
        Debug.Log("Hello");
        //Surrender (or return temporarily) until a condition is met.
        yield return new WaitForSeconds(3);
        //We will resume code here.
        Debug.Log("World");


        yield return StartCoroutine(shootTimerReset());
        Debug.Log("!");


        yield return new WaitWhile(() => canShoot == true);
        Debug.Log("canShoot is true");
        yield return new WaitUntil(() => canShoot == false);
        Debug.Log("You fired!");
    }

    public IEnumerator shootTimerReset()
    {   
        yield return new WaitForSeconds(shootTimer);
        canShoot = true;
    }

    // Update is called once per frame
	void Update ()
    {
        //Stop using update as much.
        //Still have to use it a little bit.
	}


}