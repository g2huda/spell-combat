using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	private enum EnemyState
	{
		Idle,
		Patrol,
		Investigate,
		Chase,
		Reposition,
		Attack,
		Recover
	}

	[Signal] public delegate void OnAbilityReceivedEventHandler(AbilityHitData hitData);
	[Signal] public delegate void OnDeathEventHandler(Enemy enemy);

	[Export] protected EnemyAnimationController AnimationController;
	[Export] protected float MoveSpeed = 0.5f;
	[Export] protected float PatrolRadius = 5.0f;
	[Export] protected float WaitAtPointTime = 1.5f;
	[Export] protected float InvestigateTime = 2.0f;
	[Export] protected float RotationSpeed = 6.0f;
	[Export] protected HealthBarUI HealthBar;

	private EnemyState _state = EnemyState.Idle;

	private Vector3 _spawnPosition;
	private Vector3 _targetPoint;
	private float _speed;

	private RandomNumberGenerator _rng = new();
	private float _stateTimer;

	private int _maxHealth = 100;
	private int _currentHealth;

	public void Init(Vector3 spawnPos)
	{
		_spawnPosition = spawnPos;
		GlobalPosition = _spawnPosition;
		_currentHealth = _maxHealth;
		_rng.Randomize();

		ChooseNextPatrolPoint();
		ChangeState(EnemyState.Patrol);

		AnimationController.OnDeathAnimationComplete += OnDeathCompleteHandler;
	}

	public override void _ExitTree()
	{
		AnimationController.OnDeathAnimationComplete -= OnDeathCompleteHandler;
		base._ExitTree();
	}
	public override void _PhysicsProcess(double delta)
	{
		_stateTimer -= (float)delta;
		ProcessState((float)delta);
		MoveAndSlide();
	}
	private void ProcessState(float dt)
	{
		switch(_state)
		{
			case EnemyState.Idle:
				Velocity = new Vector3(0, Velocity.Y, 0);
				if(_stateTimer <= 0.0f)
				{
					ChooseNextPatrolPoint();
					ChangeState(EnemyState.Patrol);
				}
				break;

			case EnemyState.Patrol:
				MoveTowards(_targetPoint, dt);

				if(IsCloseTo(_targetPoint, 0.5f))
				{
					ChangeState(EnemyState.Idle);
					_stateTimer = WaitAtPointTime + _rng.RandfRange(0.0f, 1.0f);
				}
				break;
		}
	}

	private void MoveTowards(Vector3 target, float dt)
	{
		Vector3 direction = target - GlobalPosition;
		direction.Y = 0.0f;

		if(direction.LengthSquared() > 0.001f)
		{
			direction = direction.Normalized();
			Velocity = new Vector3(direction.X * _speed, Velocity.Y, direction.Z * _speed);
			FaceTowards(target, dt);
		}
		else
		{
			Velocity = new Vector3(0, Velocity.Y, 0);
		}
	}

	private void FaceTowards(Vector3 target, float dt)
	{
		Vector3 lookDirection = GlobalPosition - target;
		lookDirection.Y = 0.0f;

		if(lookDirection.LengthSquared() < 0.001f)
			return;

		float targetYaw = Mathf.Atan2(lookDirection.X, lookDirection.Z);
		Vector3 rotation = Rotation;
		rotation.Y = Mathf.LerpAngle(rotation.Y, targetYaw, RotationSpeed * dt);
		Rotation = rotation;
	}

	private bool IsCloseTo(Vector3 target, float tolerance)
	{
		Vector3 a = GlobalPosition;
		Vector3 b = target;
		a.Y = 0;
		b.Y = 0;
		return a.DistanceTo(b) <= tolerance;
	}

	private void ChooseNextPatrolPoint()
	{
		float angle = _rng.RandfRange(0.0f, Mathf.Tau);
		float radius = _rng.RandfRange(1.5f, PatrolRadius);

		Vector3 offset = new Vector3(
			Mathf.Cos(angle) * radius,
			0,
			Mathf.Sin(angle) * radius
		);

		_targetPoint = _spawnPosition + offset;
	}

	private void ChangeState(EnemyState newState)
	{
		_state = newState;

		switch(_state)
		{
			case EnemyState.Idle:
				_speed = 0.0f;
				_stateTimer = WaitAtPointTime + _rng.RandfRange(0.2f, 1.0f);
				break;
			case EnemyState.Patrol:
				_speed = MoveSpeed + _rng.RandfRange(-0.3f, 0.5f);
				break;
		}

		AnimationController.SetMoveSpeed(_speed);
	}

	internal void TryReceiveAbility(AbilityHitData hitData)
	{
		if (hitData.Target == this)
		{
			EmitSignal(SignalName.OnAbilityReceived, hitData);
			// Apply damage or other effects here
			_currentHealth -= (int)hitData.Power;
			_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
			float healthPercent = (float)_currentHealth / _maxHealth;
			HealthBar.SetHealthPercent(healthPercent);
			if(_currentHealth <= 0)
			{
				AnimationController.Die();
			}
		}
	}

	private void OnDeathCompleteHandler()
	{
		EmitSignal(SignalName.OnDeath, this);
		QueueFree();
	}
}
