using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ReleaseGrabInteractable : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public XRController leftHandController;

    void Update()
    {
        // Verifica se o botão X do controle da mão esquerda foi pressionado
        if (leftHandController.inputDevice.IsPressed(InputHelpers.Button.PrimaryButton, out bool isPressed) && isPressed)
        {
            ReleaseObject();
        }
    }

    private void ReleaseObject()
    {
        if (grabInteractable.isSelected)
        {
            // Obtém o interactor que está segurando o objeto
            var interactor = grabInteractable.GetOldestInteractorSelecting();
            if (interactor != null)
            {
                // Usa o interaction manager para soltar o objeto
                grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
            }
        }
    }
}
