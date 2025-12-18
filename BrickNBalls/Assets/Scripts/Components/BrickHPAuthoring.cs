using Unity.Entities;
using UnityEngine;


public struct BrickHP : IComponentData 
{
    public int HP;
}

public class BrickHPAuthoring : MonoBehaviour
{
    [SerializeField] private int _hp;

    class Baker : Baker<BrickHPAuthoring>
    {
        public override void Bake(BrickHPAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickHP
            {
                HP = authoring._hp
            });
        }
    }
}
