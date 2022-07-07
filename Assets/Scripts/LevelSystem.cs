using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;
    public GameObject[] levels;
    GameObject tempLevel;
    int levelIndex;
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        
        levelIndex = PlayerPrefs.GetInt("LevelIndex");
        LoadLevel();
    }
    public void LoadLevel()
    {
        levelIndex = PlayerPrefs.GetInt("LevelIndex");
        if (tempLevel == null)
        {
            tempLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity);
        }else
        {
            Destroy(tempLevel);
            tempLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity);
        }
       
    }
    public void SetLevel(int index)
    {
        if (index >= levels.Length)
        {
            index = levels.Length - 1;
            Destroy(tempLevel);
            tempLevel = Instantiate(levels[index], transform.position, Quaternion.identity);
            PlayerPrefs.SetInt("LevelIndex", index);
        }
        else
        {
            Destroy(tempLevel);
            tempLevel = Instantiate(levels[index], transform.position, Quaternion.identity);
            PlayerPrefs.SetInt("LevelIndex", index);
        }
        
    }
}
