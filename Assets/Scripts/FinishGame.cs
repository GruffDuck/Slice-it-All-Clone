using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            transform.parent.GetComponentInParent<PlayerController>().enabled = false;
            transform.parent.GetComponentInParent<Rigidbody>().isKinematic = true;
            Manager.Instance.finishPanel.SetActive(true);
            Manager.Instance.scoreText.gameObject.SetActive(false);
            Manager.Instance.lastScore.text = (SliceParts.instance.score * other.gameObject.GetComponent<FinishText>().random).ToString();

        }else if (other.gameObject.CompareTag("None"))
        {
            transform.parent.GetComponentInParent<PlayerController>().enabled = false;
            transform.parent.GetComponentInParent<Rigidbody>().isKinematic = true;
            Manager.Instance.finishPanel.SetActive(true);
            Manager.Instance.scoreText.gameObject.SetActive(false);
            Manager.Instance.lastScore.text = (SliceParts.instance.score).ToString();

        }
    }
    public void SetLevel(int index)
    {
        GameObject.Find("Universal Manager").GetComponent<LevelSystem>().SetLevel(index);
    }
}
