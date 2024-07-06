using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    IEnumerator A()
    {
        Debug.Log(3);
        yield return B();
        Debug.Log(3);
    }

    IEnumerator B()
    {
        yield return new WaitForSeconds(8);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(A());
    }
}
