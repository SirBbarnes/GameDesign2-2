using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Network_Transform : NetworkComponent
{
    public Vector3 lastPos = Vector3.zero;
    public Vector3 lastRot = Vector3.zero;
    public Vector3 lastVel = Vector3.zero;
    public Vector3 lastAng = Vector3.zero;

    int count;

    public override void HandleMessage(string flag, string value)
    {
        //(x,y,z)
        char[] removeParen = {'(',')'};
        if (flag == "Position")
        {
            //naive approach
            string[] data = value.Trim(removeParen).Split(',');

            //If you have rigid body
            //Find the distance between client position and server update position
            //If distancd is less than threshold -- ignore
            //else if distance is less than threshold -- lerp
            //else -- teleport

            Vector3 target = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));

            if((target - this.transform.position).magnitude < -.5f)
            {
                //lerp
                this.transform.position = Vector3.Lerp(this.transform.position, target, 0.25f);
            }

            else
            {
                this.transform.position = target;
            }

        }

        if (flag == "Rotation")
        {
            string[] data = value.Trim(removeParen).Split(',');
            Vector3 euler = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            this.transform.rotation = Quaternion.Euler(euler);
        }

        if (flag == "Velocity")
        {
            string[] data = value.Trim(removeParen).Split(',');
            Vector3 velocity = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            this.gameObject.GetComponent<Rigidbody>().velocity = velocity;
        }

        if (flag == "Angular Velocity")
        {
            string[] data = value.Trim(removeParen).Split(',');
            Vector3 angular = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            this.gameObject.GetComponent<Rigidbody>().angularVelocity = angular;
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while(IsServer)
        {
            //Is the position different?
            if (lastPos != this.transform.position)
            {
                //Send Update
                SendUpdate("Position", this.transform.position.ToString());
                lastPos = this.transform.position;
            }

            //Is the rotation different?
            if(lastRot != this.transform.rotation.eulerAngles)
            {
                SendUpdate("Rotation", this.transform.rotation.eulerAngles.ToString());
                lastRot = this.transform.rotation.eulerAngles;
            }

            if (lastVel != this.gameObject.GetComponent<Rigidbody>().velocity)
            {
                SendUpdate("Velocity", this.gameObject.GetComponent<Rigidbody>().velocity.ToString());
                lastVel = this.gameObject.GetComponent<Rigidbody>().velocity;
            }

            if (lastAng != this.gameObject.GetComponent<Rigidbody>().angularVelocity)
            {
                SendUpdate("Angular Velocity", this.gameObject.GetComponent<Rigidbody>().angularVelocity.ToString());
                lastAng = this.gameObject.GetComponent<Rigidbody>().angularVelocity;
            }

            count++;
            if (count % 20 == 0)
            {
                this.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
            }

            
            if (IsDirty)
            {
                SendUpdate("Position", this.transform.position.ToString());
                SendUpdate("Rotation", this.transform.rotation.eulerAngles.ToString());
                SendUpdate("Velocity", this.gameObject.GetComponent<Rigidbody>().velocity.ToString());
                SendUpdate("Angular Velocity", this.gameObject.GetComponent<Rigidbody>().angularVelocity.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
