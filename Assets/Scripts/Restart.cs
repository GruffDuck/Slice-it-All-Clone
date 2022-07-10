using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        GameObject.Find("Universal Manager").GetComponent<LevelSystem>().LoadLevel();
        Manager.Instance.deathPanel.SetActive(false);
    }
}
