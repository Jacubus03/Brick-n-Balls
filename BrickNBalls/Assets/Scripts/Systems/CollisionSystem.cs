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
        ComponentLookup<BrickTag> brickLookup = SystemAPI.GetComponentLookup<BrickTag>(true);
        ComponentLookup<DeadZoneTag> deadZoneLookup = SystemAPI.GetComponentLookup<DeadZoneTag>(true);
        var hitBrick = new NativeReference<Entity>(Allocator.TempJob);
        var deadZoneHits = new NativeList<Entity>(Allocator.TempJob);

        var jobHandle = new CollisionJob()
        {
            CollisionWorld = collisionWorld,
            BrickLookup = brickLookup,
            DeadZoneLookup = deadZoneLookup,
            HitBrick = hitBrick,
            DeadZoneHits = deadZoneHits
        }.Schedule(state.Dependency);

        state.Dependency = jobHandle;
        state.Dependency.Complete();

        if (!deadZoneHits.IsEmpty)
        {
            foreach (Entity hit in deadZoneHits)
            {
                state.EntityManager.DestroyEntity(hit);
            }
            deadZoneHits.Clear();
        }

        if (hitBrick.Value != Entity.Null)
        {
            var score = SystemAPI.GetSingletonRW<ScoreData>();
            score.ValueRW.Value += 1;

            if (SystemAPI.HasComponent<BrickHP>(hitBrick.Value))
            {
                var brickHp = SystemAPI.GetComponentRW<BrickHP>(hitBrick.Value);
                brickHp.ValueRW.Value -= 1;

                if (brickHp.ValueRW.Value <= 0)
                {
                    state.EntityManager.DestroyEntity(hitBrick.Value);
                }
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}

[BurstCompile]
public partial struct CollisionJob : IJobEntity
{
    [ReadOnly] public CollisionWorld CollisionWorld;
    [ReadOnly] public ComponentLookup<BrickTag> BrickLookup;
    [ReadOnly] public ComponentLookup<DeadZoneTag> DeadZoneLookup;
    public NativeReference<Entity> HitBrick;
    public NativeList<Entity> DeadZoneHits;

    [BurstCompile]
    private unsafe void Execute(ref PhysicsVelocity ballVelocity, in PhysicsCollider ballCollider, in LocalTransform localTransform, in Entity ball)
    {
        NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.TempJob);

        bool isHit = CollisionWorld.OverlapSphere(
            localTransform.Position, 
            0.5f * localTransform.Scale, 
            ref hits, 
            ballCollider.Value.Value.GetCollisionFilter());

        if (!isHit)
        {
            hits.Dispose();
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.Entity == Entity.Null) continue;

            if (DeadZoneLookup.HasComponent(hit.Entity))
            {
                DeadZoneHits.Add(ball);
                break;
            }

            ballVelocity.Linear = math.reflect(ballVelocity.Linear, hit.SurfaceNormal);

            if (BrickLookup.HasComponent(hit.Entity))
            {
                HitBrick.Value = hit.Entity;
                break;
            }
        }

        hits.Dispose();
    }
}