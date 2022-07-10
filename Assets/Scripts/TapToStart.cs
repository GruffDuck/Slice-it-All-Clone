using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToStart : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(tap());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
        }
    }
    private IEnumerator tap()
    {
        yield return new WaitForSeconds(.3f);
        Time.timeScale = 0f;
    }
}
