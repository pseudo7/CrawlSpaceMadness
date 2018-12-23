using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float turnSpeed = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Utility.isGameOver)
            return;
        Handheld.Vibrate();
        AudioManager.Instance.Stop(Constants.TUNNEL_AUDIO);
        AudioManager.Instance.Stop(Constants.BACKGROUND_AUDIO);
        AudioManager.Instance.Play(Constants.PLAYER_DIE_AUDIO);
        TunnelManager.Instance.ReverseFinish(false);
        var meshRenderer = collision.collider.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard")) { color = Color.red, mainTexture = meshRenderer.material.mainTexture };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Utility.isGameOver || Utility.isPoolingOver)
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
