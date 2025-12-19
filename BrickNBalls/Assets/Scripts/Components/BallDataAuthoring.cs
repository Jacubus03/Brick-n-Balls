using Unity.Entities;
using UnityEngine;


public struct BallData : IComponentData 
{
    public Entity BallPrefab;
    public float Speed;
    public int Count;
}

public class BallDataAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _speed;
    [SerializeField] private int _count;

    class Baker : Baker<BallDataAuthoring>
    {
        public override void Bake(BallDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BallData
            {
                BallPrefab = GetEntity(authoring._ballPrefab, TransformUsageFlags.Dynamic),
                Speed = authoring._speed,
                Count = authoring._count
            });
        }
    }
}
