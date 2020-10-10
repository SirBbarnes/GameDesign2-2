using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTarget());
    }
    public IEnumerator MoveTarget()
    {
        while(true)
        {
            this.transform.position += transform.forward * 0.02f;
            yield return new WaitForSeconds(1f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
