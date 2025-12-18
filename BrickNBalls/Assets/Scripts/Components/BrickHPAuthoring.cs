using Unity.Entities;
using UnityEngine;


public struct BrickHP : IComponentData 
{
    public int Value;
}

public class BrickHPAuthoring : MonoBehaviour
{
    [SerializeField] private int _value;

    class Baker : Baker<BrickHPAuthoring>
    {
        public override void Bake(BrickHPAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickHP
            {
                Value = authoring._value
            });
        }
    }
}
