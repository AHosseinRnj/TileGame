using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPersist : MonoBehaviour
{
    void Awake()
    {
        int levelPersistsCount = FindObjectsOfType<LevelPersist>().Length;

        if (levelPersistsCount > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    public void ResetLevelPersist()
    {
        Destroy(gameObject);
    }
}
