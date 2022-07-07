using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject finishPanel;
    public GameObject deathPanel;
    [Header("FinishPanel Variables")]
    public TextMeshProUGUI lastScore;
    private void Awake()
    {
        Instance=this;
    }

}
