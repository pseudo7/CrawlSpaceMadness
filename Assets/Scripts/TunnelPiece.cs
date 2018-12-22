using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPiece : MonoBehaviour
{
    private void Update()
    {
        transform.position += transform.forward * Utility.movingSpeed * -Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (!Utility.isPoolingOver)
            if (transform.position.z < -3)
                //{
                transform.position += new Vector3Int(0, 0, (int)transform.position.z) * Constants.POOL_SIZE;
            //    return;
            //}
            else if (transform.position.z > 17)
                transform.position += Vector3.back * Constants.POOL_SIZE;
    }
}