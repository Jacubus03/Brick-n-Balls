using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct GameOverSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BallData>();
        state.RequireForUpdate<GameStateData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ballData = SystemAPI.GetSingleton<BallData>();
        if (ballData.Count > 0) return;

        foreach (var ball in SystemAPI.Query<BallTag>()) return;

        var gameStateData = SystemAPI.GetSingletonRW<GameStateData>();
        gameStateData.ValueRW.State = GameState.GameOver;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}