using Unity.Entities;
using UnityEngine;


public struct DeadZoneTag : IComponentData { }

public class DeadZoneAuthoring : MonoBehaviour
{
    class Baker : Baker<DeadZoneAuthoring>
    {
        public override void Bake(DeadZoneAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new DeadZoneTag());
        }
    }
}
