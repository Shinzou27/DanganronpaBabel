using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabVelocityTracked : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        SetParentToXRRig(args.interactorObject);
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        SetParentToWorld();
        base.OnSelectExited(args);
    }

    private void SetParentToXRRig(IXRSelectInteractor interactor)
    {
        transform.SetParent(interactor.transform);
    }

    private void SetParentToWorld()
    {
        transform.SetParent(null);
    }
}
