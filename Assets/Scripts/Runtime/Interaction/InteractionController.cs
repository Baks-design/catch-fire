using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] CharacterData data;
        [SerializeField] InteractionData interactionData;
        Camera cam;
        Ray ray;
        RaycastHit hitInfo;
        IPlayerMapInput inputProvider;
        IInteractionUIService interactionUIService;
        bool interacting;
        bool hitSomething;
        float holdTimer;
        bool isHolding;

        void Awake()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.InteractionActionSetup();
            cam = Camera.main;
            holdTimer = 0f;
            isHolding = false;
            interacting = false;
        }

        void OnEnable()
        {
            inputProvider.InteractionPressed.started += InteractionInput;
            inputProvider.InteractionPressed.performed += InteractionInput;
            inputProvider.InteractionPressed.canceled += InteractionInput;
        }

        void OnDisable()
        {
            inputProvider.InteractionPressed.started -= InteractionInput;
            inputProvider.InteractionPressed.performed -= InteractionInput;
            inputProvider.InteractionPressed.canceled -= InteractionInput;
        }

        void InteractionInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    interacting = true;
                    holdTimer = 0f;
                    break;
                case InputActionPhase.Performed:
                    isHolding = true;
                    break;
                case InputActionPhase.Canceled:
                    interacting = false;
                    isHolding = false;
                    holdTimer = 0f;
                    interactionUIService.UpdateProgressBar(0f);
                    break;
            }
        }

        void Start() => ServiceLocator.Global.Get(out interactionUIService);

        void Update()
        {
            CheckForInteractable();
            HandleInteraction();
        }

        void CheckForInteractable()
        {
            ray = new Ray(cam.transform.position, cam.transform.forward);

            hitSomething = Physics.SphereCast(
                ray, data.raySphereRadius, out hitInfo, data.rayDistance,
                data.interactableLayer, QueryTriggerInteraction.Ignore);

            if (hitSomething)
            {
                if (hitInfo.transform.TryGetComponent<InteractableBase>(out var interactable))
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = interactable;
                        interactionUIService.SetTooltip(interactable.TooltipMessage);
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(interactable))
                        {
                            interactionData.Interactable = interactable;
                            interactionUIService.SetTooltip(interactable.TooltipMessage);
                        }
                    }
                }
            }
            else
            {
                interactionUIService.ResetUI();
                interactionData.ResetData();
            }
        }

        void HandleInteraction()
        {
            if (!interacting && !interactionData.Interactable.IsInteractable) return;

            if (isHolding)
            {
                holdTimer += Time.deltaTime;

                var heldPercent = holdTimer / interactionData.Interactable.HoldDuration;
                if (heldPercent > 1f)
                {
                    interactionData.Interact();
                    interacting = false;
                }

                interactionUIService.UpdateProgressBar(heldPercent);
            }
            else
            {
                interactionData.Interact();
                interacting = false;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(ray.origin, data.raySphereRadius);

            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * data.rayDistance);

            if (hitSomething)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hitInfo.point, data.raySphereRadius);

                Gizmos.DrawLine(ray.origin, hitInfo.point);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal * 0.5f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(ray.origin + ray.direction * data.rayDistance, data.raySphereRadius);
            }
        }
    }
}