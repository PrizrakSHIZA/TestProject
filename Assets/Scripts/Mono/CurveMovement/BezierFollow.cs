using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
   public Transform[] routes = new Transform[3];
   public float speed = 0.5f;

    Vector3 targetPosition;
    int routeToGo = 0;
    float tParam = 0f;
    bool coroutineAllowed = true;

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routenumber)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routenumber].GetChild(0).position;
        Vector3 p1 = routes[routenumber].GetChild(1).position;
        Vector3 p2 = routes[routenumber].GetChild(2).position;
        Vector3 p3 = routes[routenumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += speed * Time.deltaTime;

            targetPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            gameObject.transform.LookAt(targetPosition);

            transform.position = targetPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;

        coroutineAllowed = true;
    }
}
