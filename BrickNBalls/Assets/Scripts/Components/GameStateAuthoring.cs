using Unity.Entities;
using UnityEngine;


public enum GameState
{
    Playing,
    GameOver
}

public struct GameStateData : IComponentData
{
    public GameState State;
}

public class GameStateAuthoring : MonoBehaviour
{
    class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new GameStateData { State = GameState.Playing });
        }
    }
}
