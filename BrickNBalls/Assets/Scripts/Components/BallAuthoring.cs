using Unity.Entities;
using UnityEngine;


public struct BallTag : IComponentData { }

public class BallAuthoring : MonoBehaviour
{
    class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BallTag());
        }
    }
}
