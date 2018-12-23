using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TunnelManager : MonoBehaviour
{
    public static TunnelManager Instance;

    [Header("Controller")]
    public Transform ballTransform;
    public Transform movingTransform;
    public List<GameObject> obstaclesList;

    static Camera mainCam;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        obstaclesList = new List<GameObject>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        for (int i = 1; i <= Constants.POOL_SIZE; i++)
        {
            var poolObject = ObjectPooler.SharedInstance.GetPooledObject(0);
            poolObject.SetActive(true);
            if (i % 5 == 0)
                poolObject.GetComponent<TunnelPiece>().isObstacleEnabled = true;
            var poolObjectTransform = poolObject.transform;
            poolObjectTransform.rotation = Quaternion.identity;
            poolObjectTransform.Rotate(0, 0, i * 30);
            poolObjectTransform.position = new Vector3(0, 0, i);
        }
    }

    public void ReverseFinish()
    {
        foreach (var obstacle in obstaclesList)
        {
            obstacle.GetComponentInParent<TunnelPiece>().isObstacleEnabled = false;
            obstacle.SetActive(false);
        }
        LeanTween.value(gameObject, ModifyMovingSpeed, Utility.movingSpeed, -5, 5).setEaseInOutSine();
        LeanTween.move(ballTransform.gameObject, Vector3.zero, 2f);
        LeanTween.delayedCall(5, () => { CurvatureController.Instance.CrossFadeCurvature(Vector2.zero); })
            .setOnComplete(() => { LeanTween.value(mainCam.gameObject, ModifyFOV, mainCam.fieldOfView, 179, 2).setEaseOutSine(); });
        ;
    }

    void ModifyFOV(float fov)
    {
        Camera.main.fieldOfView = fov;
    }

    void ModifyMovingSpeed(float val)
    {
        Utility.movingSpeed = val;
    }
}
