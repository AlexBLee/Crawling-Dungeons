using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveUp : MonoBehaviour
{
    private const float moveSpeed = 3.0f;
    private const int destroyTime = 2;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, moveSpeed * Time.deltaTime);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
        
    }
}
