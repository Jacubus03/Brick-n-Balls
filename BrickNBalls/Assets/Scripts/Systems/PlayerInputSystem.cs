using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine.InputSystem;

public partial class PlayerInputSystem : SystemBase
{
    private InputSystem _inputSystem;
    private float _moveInput;
    private bool _attackInput;

    protected override void OnCreate()
    {
        RequireForUpdate<BallData>();
        _inputSystem = new InputSystem();
        _inputSystem.Enable();

        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += OnMove;
        _inputSystem.Player.Attack.started += OnAttack;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = -ctx.ReadValue<float>();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        _attackInput = true;
    }

    protected override void OnUpdate() 
    {
        if (_moveInput == 0 && !_attackInput) return;

        float delta = _moveInput * SystemAPI.Time.DeltaTime;

        var ballData = SystemAPI.GetSingletonRW<BallData>();

        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<PlayerTag>())
        {
            float3 euler = math.Euler(transform.ValueRO.Rotation);
            euler.z = math.clamp(euler.z + delta, math.radians(-80f), math.radians(80f));
            transform.ValueRW.Rotation = quaternion.Euler(euler);

            if (_attackInput && ballData.ValueRO.Count > 0)
            {
                Entity ball = EntityManager.Instantiate(ballData.ValueRO.BallPrefab);

                LocalTransform transformData = EntityManager.GetComponentData<LocalTransform>(ball);
                transformData.Position = transform.ValueRW.Position;
                transformData.Position.y += 0.1f;
                EntityManager.SetComponentData(ball, transformData);

                float3 up = math.mul(transform.ValueRW.Rotation, new float3(0f, 1f, 0f));
                EntityManager.SetComponentData(ball, new PhysicsVelocity
                {
                    Linear = up * ballData.ValueRO.Speed,
                    Angular = float3.zero
                });

                ballData.ValueRW.Count--;
            }
        }

        _attackInput = false;
    }

    protected override void OnDestroy()
    {
        _inputSystem.Player.Move.performed -= OnMove;
        _inputSystem.Player.Move.canceled -= OnMove;
        _inputSystem.Player.Attack.started -= OnAttack;
    }
}
