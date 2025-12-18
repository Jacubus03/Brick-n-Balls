using Unity.Entities;
using UnityEngine;


public struct WallTag : IComponentData { }

public class WallAuthoring : MonoBehaviour
{
    class Baker : Baker<WallAuthoring>
    {
        public override void Bake(WallAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new WallTag());
        }
    }
}
