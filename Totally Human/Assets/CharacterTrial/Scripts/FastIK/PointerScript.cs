using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    public float armDist;//for scroll

    //public Transform floorMat;
    private Vector3 newPos;
    //network
    public Transform targetTransform;
    public Transform mainGuy;
    public Transform camTarget;
    public float camUp;
    public float camFrwd;
    public float speed;
    public float handSpeed = 0.03f;
    public float offset;
    public bool isFoot;


    private Ray ray;
    private RaycastHit axisHit;
    private RaycastHit hit;
    private LayerMask mask;
    private Vector3 mover;
    private Vector3 originalAxis;
    private float distanceToMaintain;
    private float tempAngle;
    private Vector3 camNewPos;

    void Start()
    {
        //targetTransform = target.transform;
        StartCoroutine(MoveTarget());
        mask = LayerMask.GetMask("floor");
        //distanceToMaintain = (originalAxis - rightFoot.position).magnitude;

        //maxDistSqrd = maxDist * maxDist;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator MoveTarget()
    {
        while(true)
        {
            /*
            newPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));

            if (isFoot)
            {
                newPos += (newPos - targetTransform.position).normalized * maxDist;
            }
            targetTransform.position = Vector3.Lerp(newPos, targetTransform.position, 0.7f);
            //Debug.DrawRay(homeBase.position, targetTransform.position, Color.green);
            */

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin+Vector3.forward*3, Color.green);

            if (Input.GetMouseButton(0)) //leftclick
            {
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                    targetTransform.position = Vector3.Lerp(targetTransform.position, hit.point, 0.5f);
                }
            }

            if (Input.GetMouseButton(1)) //rightclick
            {
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                    mover = (hit.point - mainGuy.position).normalized;
                    mover = new Vector3(mover.x, 0f, mover.z);
                    mainGuy.Translate(mover * Time.deltaTime * speed);

                    //tempAngle = Vector3.Angle(mainGuy.forward, hit.point- mainGuy.forward);
                    //mainGuy.RotateAround(mainGuy.position, mainGuy.up, tempAngle);


                    //Debug.DrawLine(mainGuy.forward, hit.point, Color.red);
                    Debug.DrawLine(mainGuy.position, mainGuy.forward, Color.green);
                }
            }
            Debug.DrawLine(mainGuy.position, mainGuy.forward, Color.red);


            //if(Physics.Raycast(ray,out hit, 100, mask))
            //{
            //mover = floorMat.InverseTransformPoint(hit.point);
            //temp = mover.magnitude;
            /*
            if (temp < 0)
                temp = 0;
            offset = -1.0f * temp * (temp - 1.5f); // quadratic function for leg lift and descnet
            */
            //scalar = mover.magnitude;
            //scalar *= scalar*offset;


            //targetTransform.position = Vector3.Lerp(targetTransform.position, hit.point, 0.5f);
            /*
            tempH = Mathf.Cos(Vector3.Angle(originalAxis - rightFoot.position, originalAxis - leftFoot.position) * 0.5f);
            if (tempH <= 0)
                tempH = 1f;
            axis.position = new Vector3( (rightFoot.position.x + leftFoot.position.x)*0.5f
                                        , distanceToMaintain * tempH,
                                        (rightFoot.position.z + leftFoot.position.z) * 0.5f);
            */
            //axis.position = (rightFoot.position + targetTransform.position) * 0.5f;
            /*
            axis.position = (leftFoot.position + rightFoot.position) * 0.5f
                    + Vector3.up* 1/((leftFoot.position - rightFoot.position).magnitude);

            if (originalAxis.y <= axis.position.y)
                axis.position = originalAxis;

            */
            //Debug.DrawLine(ray.origin, hit.point, Color.green);

            //handRig.MovePosition(hit.point);


            //transform.Translate(Vector3.forward * Time.deltaTime);
            //}
            camNewPos = -1f * camFrwd * mainGuy.forward + camUp * mainGuy.up + mainGuy.position;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camNewPos, 0.7f);
            Camera.main.transform.LookAt(camTarget.position);

            //1.66
            //1.54

            yield return new WaitForSeconds(handSpeed);
        }
    }
}
