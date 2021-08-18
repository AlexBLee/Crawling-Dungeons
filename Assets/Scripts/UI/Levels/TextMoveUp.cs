using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveUp : MonoBehaviour
{
    void Update()
    {
        const float MoveSpeed = 3.0f;

        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, MoveSpeed * Time.deltaTime);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        const int DestroyTime = 2;

        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}
