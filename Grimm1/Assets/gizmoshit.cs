using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmoshit : MonoBehaviour


{
    [SerializeField]
    private float maxFalloffDist;

    [SerializeField]
    private float minFalloffDist;
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxFalloffDist);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minFalloffDist);
    }
}
