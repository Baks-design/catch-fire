using UnityEngine;

namespace CatchFire
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] bool holdInteract = true;
        [SerializeField] float holdDuration = 1f;
        [SerializeField] bool multipleUse = false;
        [SerializeField] bool isInteractable = true;
        [SerializeField] string tooltipMessage = "interact";

        public float HoldDuration => holdDuration;
        public bool HoldInteract => holdInteract;
        public bool MultipleUse => multipleUse;
        public bool IsInteractable => isInteractable;
        public string TooltipMessage => tooltipMessage;

        public virtual void OnInteract() => Debug.Log("INTERACTED: " + gameObject.name);
    }
}
