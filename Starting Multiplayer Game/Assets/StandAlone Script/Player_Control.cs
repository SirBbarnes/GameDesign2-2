using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Player_Control : NetworkComponent
{
    public Vector3 lastInput = Vector3.zero;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "MOVE")
        {
            char[] removeParen = { '(', ')' };
            string[] data = value.Trim(removeParen).Split(',');

            lastInput = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while(true)
        {
            if (IsLocalPlayer)
            {
                Vector3 temp = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

                if ((temp - lastInput).magnitude > 0.01f)
                {
                    SendCommand("MOVE", temp.ToString());
                    lastInput = temp;
                    Debug.Log("Sending a Command!");
                }
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Only on Client Side
        //Graphical Effects
        if (IsServer)
        {
            this.transform.position += lastInput * Time.deltaTime;
        }
    }
}


/*
 * This is how collisions/triggers would work
 *
 *
 * 
public void OnCollisionEnter(Collision collision)
{
    if(IsServer)
    {
        if(collision.gameObject.tag == "BAD")
        {
            //HP -= 1;
            //SendUpdate
        }
    }

    if(IsClient)
    {
        if(collision.gameObject.tag == "BAD")
        {
            //show explosion
        }
    }

    if(IsLocalPlayer)
    {
        if(collision.gameObject.tag == "BAD")
        {
            //shake screen
        }
    }
}
*/
