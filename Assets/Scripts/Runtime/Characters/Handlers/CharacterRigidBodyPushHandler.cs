using UnityEngine;

namespace CatchFire
{
	public class CharacterRigidBodyPushHandler : IRigidBodyPush
	{
		readonly CharacterData data;

		public CharacterRigidBodyPushHandler(CharacterData data) => this.data = data;

		public void PushRigidBodies(ControllerColliderHit hit)
		{
			if (!hit.collider.TryGetComponent(out Rigidbody body) || body.isKinematic) return;

			var bodyLayer = body.gameObject.layer;
			if ((data.pushLayers.value & (1 << bodyLayer)) == 0) return;

			if (hit.moveDirection.y < -0.3f) return;

			var pushDir = hit.moveDirection;
			pushDir.y = 0f;

			body.AddForce(pushDir * data.strength, ForceMode.Impulse);
		}
	}
}