using Unity.Entities;
using UnityEngine;


public struct ScoreData : IComponentData
{
    public int Value;
}

public class ScoreAuthoring : MonoBehaviour
{
    class Baker : Baker<ScoreAuthoring>
    {
        public override void Bake(ScoreAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ScoreData { Value = 0 });
        }
    }
}
