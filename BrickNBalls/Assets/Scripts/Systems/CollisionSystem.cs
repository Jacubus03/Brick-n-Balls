using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct CollisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        state.RequireForUpdate<ScoreData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        NativeReference<bool> isBrickHit = new NativeReference<bool>(Allocator.TempJob);

        var jobHandle = new CollisionJob()
        {
            CollisionWorld = collisionWorld,
            IsBrickHit = isBrickHit
        }.Schedule(state.Dependency);

        state.Dependency = jobHandle;
        state.Dependency.Complete();

        if (isBrickHit.Value)
        {
            var score = SystemAPI.GetSingletonRW<ScoreData>();
            score.ValueRW.Value += 1;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}

[BurstCompile]
public partial struct CollisionJob : IJobEntity
{
    [ReadOnly] public CollisionWorld CollisionWorld;
    public NativeReference<bool> IsBrickHit;

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
        IsBrickHit.Value = true;

        hits.Dispose();
    }
}