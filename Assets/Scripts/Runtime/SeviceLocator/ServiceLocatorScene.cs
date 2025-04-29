using UnityEngine;

namespace CatchFire
{
    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class ServiceLocatorScene : Bootstrapper
    {
        protected override void Bootstrap() => Container.ConfigureForScene();
    }
}