using UnityEngine;

namespace CatchFire
{
	public class CharacterRigidBodyPushHandler : IRigidBodyPush
	{
		readonly CharacterData data;

		public CharacterRigidBodyPushHandler(CharacterData data) => this.data = data;

		public void PushRigidBodies(ControllerColliderHit hit)
		{
			// Early exit if no rigidbody or kinematic
			if (!hit.collider.TryGetComponent(out Rigidbody body) || body.isKinematic) return;

			// Cache layer check
			int bodyLayer = body.gameObject.layer;
			if ((data.pushLayers.value & (1 << bodyLayer)) == 0) return;

			// Early exit if pushing downward (with epsilon for floating point comparison)
			const float minYThreshold = -0.3f;
			if (hit.moveDirection.y < minYThreshold) return;

			// Calculate horizontal push direction (no allocation)
			var pushDir = hit.moveDirection;
			pushDir.y = 0f;

			// Apply force with cached strength
			body.AddForce(pushDir * data.strength, ForceMode.Impulse);
		}
	}
}