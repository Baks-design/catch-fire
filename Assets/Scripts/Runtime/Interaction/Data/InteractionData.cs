using UnityEngine;

namespace CatchFire
{    
    [CreateAssetMenu( menuName = "Data/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        InteractableBase interactable;

        public InteractableBase Interactable
        {
            get => interactable;
            set => interactable = value;
        }

        public void Interact()
        {
            interactable.OnInteract();
            ResetData();
        }

        public bool IsSameInteractable(InteractableBase newInteractable) => interactable == newInteractable;
        
        public bool IsEmpty() => interactable == null;

        public void ResetData() => interactable = null;
    }
}
