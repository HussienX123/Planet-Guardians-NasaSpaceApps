using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
public class RecenterOrgion : MonoBehaviour
{

    [SerializeField] Transform Start_lookAt;
    [SerializeField] Transform Start_Position;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Recenter();
    }

    [ContextMenu("center me please")]
    public void Recenter()
    {
        XROrigin xRorigin = GetComponent<XROrigin>();
        if(Start_Position)
            xRorigin.MoveCameraToWorldLocation(Start_Position.position);

        if (Start_lookAt)
            xRorigin.MatchOriginUpCameraForward(Start_lookAt.up, Start_lookAt.forward);
    }
}
