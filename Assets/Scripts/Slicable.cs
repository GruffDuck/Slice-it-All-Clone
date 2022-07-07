using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slicable : MonoBehaviour
{
    public static Slicable instance;
     Rigidbody[] rb;
    private ParticleSystem particle;
    public int scoreCount;
   
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        particle = GetComponentInChildren<ParticleSystem>();
        rb = GetComponentsInChildren<Rigidbody>();
        instance = this;
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = "+" + scoreCount;
    }

    public void Slice()
    {
        foreach (var part in rb)
        {
            
            part.isKinematic = false;
            part.AddForce(part.transform.localPosition.y * 10000f * Vector3.up);
            part.transform.parent.tag = "Untagged";

        }
        particle.Play();
        StartCoroutine(ShowText());
        

    }
    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(0.5f);
        canvas.gameObject.SetActive(false);
    }

}
