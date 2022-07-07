using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishText : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    [HideInInspector]public int random;
    private void Awake()
    {
        scoreText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(); ;
    }
    void Start()
    {
        switch (gameObject.name)
        {
            case "Easy":
                 random = Random.Range(1, 3);
                scoreText.text = "X" + random;
                break;
            case "Normal":
                 random = Random.Range(2, 4);
                scoreText.text = "X" + random;
                break;
            case "Hard":
                random = Random.Range(3, 5);
                scoreText.text = "X" + random;
                break;
            case "Xtreme":
                random = Random.Range(5, 8);
                scoreText.text = "X" + random;
                break;
        }
    }


}
