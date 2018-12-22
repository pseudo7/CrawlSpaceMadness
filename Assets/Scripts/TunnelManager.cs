using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TunnelManager : MonoBehaviour
{
    public static TunnelManager Instance;

    [Header("Controller")]
    //public float movingSpeed = 2f;
    public Transform ballTransform;
    public Transform movingTransform;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < Constants.POOL_SIZE; i++)
        {
            var poolObject = ObjectPooler.SharedInstance.GetPooledObject(0);
            poolObject.SetActive(true);
            var poolObjectTransform = poolObject.transform;
            poolObjectTransform.rotation = Quaternion.identity;
            poolObjectTransform.Rotate(0, 0, i * 30);
            poolObjectTransform.position = new Vector3(0, 0, i);
        }
    }

    public void ReverseFinish()
    {
        LeanTween.value(gameObject, UpdateValue, Utility.movingSpeed, -5, 5).setEaseInOutSine();
        LeanTween.move(ballTransform.gameObject, Vector3.zero, 2f);
        LeanTween.delayedCall(5, () => { CurvatureController.Instance.CrossFadeCurvature(Vector2.zero); });
    }

    void UpdateValue(float val)
    {
        Utility.movingSpeed = val;
    }

    public void Update()
    {
        //movingTransform.position += movingTransform.forward * movingSpeed * -Time.deltaTime;
    }
}
