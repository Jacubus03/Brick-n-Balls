using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

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
    private unsafe void Execute(ref PhysicsVelocity ballVelocity, in PhysicsCollider ballCollider, in LocalTransform localTransform)
    {
        NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.TempJob);

        bool isHit = CollisionWorld.OverlapSphere(
            localTransform.Position, 
            0.5f * localTransform.Scale, 
            ref hits, 
            ballCollider.Value.Value.GetCollisionFilter());

        if (!isHit) return;

        ballVelocity.Linear = math.reflect(ballVelocity.Linear, hits[0].SurfaceNormal);

        hits.Dispose();
    }
}