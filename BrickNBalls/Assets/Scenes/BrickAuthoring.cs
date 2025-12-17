using Unity.Entities;
using Unity.Physics;
using UnityEngine;


public struct BrickTag : IComponentData { }

public class BrickAuthoring : MonoBehaviour
{
    class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickTag());
        }
    }
}
