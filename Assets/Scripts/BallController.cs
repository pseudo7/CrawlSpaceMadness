using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    public float turnSpeed = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        Handheld.Vibrate();
        SceneManager.LoadScene(0);
    }

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
        z = Input.acceleration.x * 3;
#endif
        transform.parent.Rotate(0, 0, z * Time.deltaTime * turnSpeed);
        transform.Rotate(z, z, z);
    }
}
