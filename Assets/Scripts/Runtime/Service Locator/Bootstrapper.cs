using UnityEngine;

namespace CatchFire
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        bool hasBeenBootstrapped;
        ServiceLocator container;

        internal ServiceLocator Container
        {
            get
            {
                if (container == null)
                    container = GetComponent<ServiceLocator>();
                return container;
            }
        }

        void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}
