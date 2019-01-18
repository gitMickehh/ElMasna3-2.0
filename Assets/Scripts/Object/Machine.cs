using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

    public GameObject CurrentWorker;
    public Transform workerPosition;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (workerPosition != null)
            Gizmos.DrawWireSphere(workerPosition.position, 0.5f);
        //else
        //{
        //    Debug.LogWarning("You need to add worker position when the game runs.\n" +
        //        "Becasue your workers will run towards that transform point");
        //}
    }
}
