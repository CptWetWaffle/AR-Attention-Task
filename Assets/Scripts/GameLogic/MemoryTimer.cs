using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MemoryTimer : MonoBehaviour {

    public static bool start { get; set; }
    public static float sessionTime;
    public static bool timeSessionExpired = false;
    public GameObject MemTarget;


    void Start()
    {
        if(StartUp.LevelConfig() != null)
        sessionTime = StartUp.LevelConfig().picTimer;
    }

    void Update()
    {
        if (start)
            CountTime();
    }

    void CountTime()
    {
        MemTarget.SetActive(true);
        if (sessionTime >= 0)
        {
            sessionTime -= Time.deltaTime;
            if (sessionTime <= 0)
            {
                MemTarget.SetActive(false);
                start = false;
            }

        }
    }
}
