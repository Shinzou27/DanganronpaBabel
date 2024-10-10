using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoGrabInteractable : XRGrabInteractable
{
    private IXRSelectInteractor interactor;

    private void OnTriggerEnter(Collider other)
    {
        // Detecta se a mão do jogador entrou na área de alcance
        if (other.CompareTag("PlayerHand"))
        {
            // Obtém o interactor (a mão do jogador)
            interactor = other.GetComponentInParent<XRBaseInteractor>();
            if (interactor != null)
            {
                // Automaticamente agarra o objeto
                interactionManager.SelectEnter(interactor, this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detecta se a mão do jogador saiu da área de alcance
        if (other.CompareTag("PlayerHand") && interactor != null)
        {
            // Solta o objeto
            interactionManager.SelectExit(interactor, this);
            interactor = null;
        }
    }
}
