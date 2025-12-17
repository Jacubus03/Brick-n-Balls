using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Collections;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct CollisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

        new CollisionJob()
        {
            CollisionWorld = collisionWorld
        }.Schedule();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

[BurstCompile]
public partial struct CollisionJob : IJobEntity
{
    [ReadOnly] public CollisionWorld CollisionWorld;


    [BurstCompile]
    private void Execute()
    {
        CollisionWorld.OverlapSphere();
    }
}