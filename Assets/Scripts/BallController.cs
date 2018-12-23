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
        StartCoroutine(Reverse());
    }

    IEnumerator Reverse()
    {
        LeanTween.value(gameObject, ModifyTimeScale, Time.timeScale, 0, 1f);
        CurvatureController.Instance.CrossFadeTiling(new Vector2(.1f, 1), 2f);
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ModifyTimeScale(float val)
    {
        Time.timeScale = val;
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
