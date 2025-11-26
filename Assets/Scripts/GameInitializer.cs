using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}