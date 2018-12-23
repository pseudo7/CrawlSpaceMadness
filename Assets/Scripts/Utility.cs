using UnityEngine;

public class Utility : MonoBehaviour
{
    public static bool isGameOver;
    public static bool isPoolingOver;
    public static float movingSpeed;


    private void Start()
    {
        isGameOver = isPoolingOver = false;
    }
}
