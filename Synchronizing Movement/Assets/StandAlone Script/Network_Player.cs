using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Network_Player : NetworkComponent
{
    Color tempColor, colorObj;
    MeshRenderer player;
    public string playName; 
    public int playCount;
    public GameObject playCanvas;
    GameObject temp;
    public GameObject startButton;
    int shapeArr;
    bool canSee = true;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "Color") 
        {
            if (ColorUtility.TryParseHtmlString(value, out tempColor))
            {
                colorObj = tempColor;
            }

            if (IsServer)
            {
                SendUpdate("Color", colorObj.ToString());
            }
        }

        if (flag == "Shape")
        {
            switch (value)
            {
                case "Sphere":
                    shapeArr = 5;
                    break;

                case "Cube":
                    shapeArr = 1;
                    break;

                case "Capsule":
                    shapeArr = 0;
                    break;

                case "Cylinder":
                    shapeArr = 2;
                    break;

                case "Plane":
                    shapeArr = 3;
                    break;

                case "Quad":
                    shapeArr = 6;
                    break;

            }

            if (IsServer)
            {
                SendUpdate("Shape", shapeArr.ToString());
            }
        }

        if (flag == "Name")
        {
            playName = value;

            if (IsServer)
            {
                SendUpdate("Name", playName.ToString());
            }
        }

        if (flag == "Ready")
        {
            playCount++;

            if (IsClient)
            {
                canSee = false;
                this.gameObject.SetActive(false);
            }

            if (playCount == MyCore.Connections.Count)
            {
                

                if (IsServer)
                {
                    canSee = false;
                    this.gameObject.SetActive(false);
                    temp = MyCore.NetCreateObject(shapeArr, 1, new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0));
                    GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material.SetColor("_Color", colorObj);
                    GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetComponent<TextMesh>().text = playName;
                    SendUpdate("Update Player", "1");
                }
            }
        }

        if (flag == "Update Player")
        {
            if (IsLocalPlayer)
            {
                this.gameObject.SetActive(false);
                temp = MyCore.NetCreateObject(shapeArr, 1, new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0));
                GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material.SetColor("_Color", colorObj);
                GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetComponent<TextMesh>().text = playName;
            }
            if (IsClient)
            {
                temp = MyCore.NetCreateObject(shapeArr, 1, new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0));
                GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material.SetColor("_Color", colorObj);
                GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetComponent<TextMesh>().text = playName;
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsLocalPlayer && canSee == true)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }

            if (!IsLocalPlayer)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }

            if (IsServer)
            {
                if (IsDirty)
                {
                    SendUpdate("Color", colorObj.ToString());
                    SendUpdate("Shape", shapeArr.ToString());
                    SendUpdate("Name", playName.ToString());

                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }



    public void setColor(string value)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Color", value);
        }
    }

    public void setShape(string value)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Shape", value);
        }
    }

    public void setName(string value)
    {
        if (IsLocalPlayer)
        {
            SendCommand("Name", value);
        }
    }
     
    public void startGame()
    {
        if (IsLocalPlayer)
        {
            canSee = false;
            SendCommand("Ready", playCount.ToString());
        }
    }
}
