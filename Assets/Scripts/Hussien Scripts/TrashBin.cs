using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum TrashType
{
    PLASTICBAG,
    GLASS,
    CANS,
}

public class TrashBin : MonoBehaviour
{
    public TrashType trashbinType;
    public Transform TrashGoToPoint;

    private Animator animator;
    private XRSimpleInteractable interactable;

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(PutTrash);
        interactable.hoverExited.AddListener(CloseTrash);

    }

    public void PutTrash(HoverEnterEventArgs args)
    {
        animator.SetBool("open", true);
    }

    public void CloseTrash(HoverExitEventArgs args)
    {
        animator.SetBool("open", false);
    }
}
