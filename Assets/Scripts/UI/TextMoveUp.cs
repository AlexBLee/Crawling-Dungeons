using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveUp : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0,1,0), 3.0f * Time.deltaTime);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        
    }
}
