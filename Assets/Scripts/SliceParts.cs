using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SliceParts : MonoBehaviour
{
    public static SliceParts instance;
    [HideInInspector] public int score;
    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slice"))
        {
            other.GetComponent<Slicable>().Slice();
            score = score + other.GetComponent<Slicable>().scoreCount;
            Debug.Log(score);
            Manager.Instance.scoreText.text = score.ToString();
        }
    }
}
