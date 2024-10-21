using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelMover : MonoBehaviour
{
    public Renderer Tunling;

    private void Start()
    {
        FadeOut();
    }

    public void FadeIn()
    {
        Tunling.material.SetColor("Vignette Color", Color.black);
        Tunling.material.SetFloat("_ApertureSize", 1f);
        Tunling.material.DOFloat(0, "_ApertureSize", 2f);
    }

    public void FadeOut()
    {
        Tunling.material.SetColor("Vignette Color", Color.black);
        Tunling.material.SetFloat("_ApertureSize", 0f);
        Tunling.material.DOFloat(1, "_ApertureSize", 2f);
    }

    public void MoveScene(int index)
    {
        StartCoroutine(SceneMove(index));
    }

    IEnumerator SceneMove(int index)
    {
        FadeIn();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(index);
    }
}
