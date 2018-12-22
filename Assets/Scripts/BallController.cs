using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float turnSpeed = 100f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Utility.isGameOver)
            return;
        float z;
#if UNITY_EDITOR
        z = Input.GetAxis("Horizontal");
#else
        z = Input.acceleration.x * 2;
#endif
        transform.Rotate(0, 0, z * Time.deltaTime * turnSpeed);
        transform.GetChild(0).Rotate(z, z, z);
    }
}
