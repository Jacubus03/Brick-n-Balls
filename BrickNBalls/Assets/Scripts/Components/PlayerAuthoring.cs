using Unity.Entities;
using UnityEngine;


public struct PlayerTag : IComponentData { }

public class PlayerAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new PlayerTag());
        }
    }
}
