using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPiece : MonoBehaviour
{
    public bool isObstacleEnabled;

    Transform[] obstacles;

    int previousEnabledIndex = -1;

    private void Start()
    {
        obstacles = new Transform[Constants.OBSTACLES_SIZE];
        for (int i = 0; i < Constants.OBSTACLES_SIZE; i++)
            obstacles[i] = transform.GetChild(i + 1);
        if (isObstacleEnabled)
            EnableObstacle(true);
    }

    void EnableObstacle(bool firstTime)
    {
        int index = Random.Range(0, Constants.OBSTACLES_SIZE);
        if (firstTime)
        {
            previousEnabledIndex = index;
            var obstacle = obstacles[index].gameObject;
            obstacle.SetActive(true);
            TunnelManager.Instance.obstaclesList.Add(obstacle);
        }
        else
        {
            obstacles[previousEnabledIndex].gameObject.SetActive(false);
            var obstacle = obstacles[index].gameObject;
            previousEnabledIndex = index;
            obstacle.SetActive(true);
        }
    }

    private void Update()
    {
        transform.position += transform.forward * Utility.movingSpeed * -Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (!Utility.isPoolingOver)
        {
            if (transform.position.z < -3)
            {
                transform.position += new Vector3Int(0, 0, 1) * (Constants.POOL_SIZE - 1);
                if (isObstacleEnabled)
                    EnableObstacle(false);
            }
        }
        else if (transform.position.z > 17)
            transform.position += new Vector3Int(0, 0, -1) * (Constants.POOL_SIZE - 1);
    }
}