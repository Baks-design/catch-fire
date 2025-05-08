using UnityEngine;

namespace CatchFire
{
    public class CharacterRoofChecker 
    {
        readonly CharacterData data;
        readonly CharacterController character;

        public bool IsRoofed { get; private set; }
        public RaycastHit IsRoofHit { get; private set; }

        public CharacterRoofChecker(
            CharacterData data,
            CharacterController character)
        {
            this.data = data;
            this.character = character;
        }

        public void CheckRoof()
        {
            IsRoofed = Physics.SphereCast(
                character.transform.position + (Vector3.up * data.groundedOffset),
                character.radius,
                Vector3.up,
                out var roofhit,
                data.roofCheckDistance,
                data.roofLayer,
                QueryTriggerInteraction.Ignore
            );
            IsRoofHit = roofhit;
        }
    }
}