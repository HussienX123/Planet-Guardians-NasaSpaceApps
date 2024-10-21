using Bitgem.VFX.StylisedWater;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashObject : MonoBehaviour
{
    private Rigidbody rb;

    public float Speed = 1f;

    public TrashType ObjectTType;

    XRSimpleInteractable grabInteractable;

    private Transform AttachedToTransform;

    private WateverVolumeFloater wateverVolumeFloater;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRSimpleInteractable>();
        wateverVolumeFloater = GetComponent<WateverVolumeFloater>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs arg0)
    {
        AttachedToTransform = null;

        StartCoroutine(GoBackToWater());

    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        AttachedToTransform = arg0.interactorObject.transform.GetComponent<Grabber>().GrabPoint;
        
        wateverVolumeFloater.ShouldFollowWaterSurface = false;

    }

    private void OnMouseDown()
    {
        CollectTrash();
    }

    public void CollectTrash()
    {
        TrashSpawner.instance.CleanATrash();
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (AttachedToTransform != null)
        {
            transform.position = AttachedToTransform.position;
        }
        else
        {
            rb.velocity = new Vector3(Speed, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MountianEnd"))
        {
            TrashSpawner.instance.LostATrash();
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BIN") && AttachedToTransform == null)
        {
            StopAllCoroutines();
            Debug.Log("Trash in the bin");
            wateverVolumeFloater.ShouldFollowWaterSurface = false;
            transform.DOScale(Vector3.zero, 1f);
            transform.DOMove(other.GetComponent<TrashBin>().TrashGoToPoint.position, 1f).OnComplete(() => {
                if(other.GetComponent<TrashBin>().trashbinType == ObjectTType)
                    TrashSpawner.instance.CleanATrash();
                else
                    TrashSpawner.instance.LostATrash();
                Destroy(gameObject);
            });
            
        }
    }

    IEnumerator GoBackToWater()
    {
        yield return new WaitForSeconds(0.25f);
        wateverVolumeFloater.ShouldFollowWaterSurface = true;
    }

    IEnumerator AutoDispose() {
        yield return new WaitForSeconds(7f);
        TrashSpawner.instance.LostATrash();
        Destroy(gameObject);
    }
}
