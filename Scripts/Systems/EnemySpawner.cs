using Godot;

public partial class EnemySpawner : Node
{
	[Signal] public delegate void OnEnemiesCountChangedEventHandler(int remainingEnemies);

	[Export] protected PackedScene EnemyScene;
	[Export] protected AbilitySystem AbilitySystem;
	[Export] protected int SpawnCount = 5;
	[Export] protected float SpawnRadius = 8.0f;
	[Export] protected Node3D CenterPoint;
	[Export] protected Node3D Player;

	private RandomNumberGenerator _rng = new();
	private int _remainingSpawns;
	private int RemainingSpawns
	{
		get => _remainingSpawns;
		set
		{
			_remainingSpawns = value;
			EmitSignal(SignalName.OnEnemiesCountChanged, _remainingSpawns);
		}
	}

	public override void _Ready()
	{
		_rng.Randomize();
		SpawnEnemies();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	private void SpawnEnemies()
	{
		if(EnemyScene == null)
		{
			GD.PrintErr("EnemyScene is not assigned on EnemySpawner.");
			return;
		}

		for(int i = 0; i < SpawnCount; i++)
		{
			Enemy enemy = EnemyScene.Instantiate<Enemy>();
			AddChild(enemy);
			
			SetupEnemySignals(enemy);

			Vector3 spawnPos = GetRandomPointAround(CenterPoint.GlobalPosition, SpawnRadius);
			enemy.GlobalPosition = Vector3.One * 1000; // Temporarily move enemy far away to avoid early collisions
			enemy.Init(spawnPos);
			enemy.MoveSpeed += _rng.RandfRange(-0.3f, 0.5f);
		}

		RemainingSpawns = SpawnCount;
	}

	private void OnDeathHandler(Enemy enemy)
	{
		RemainingSpawns--;
		CleanupEnemySignals(enemy);
	}

	private void SetupEnemySignals(Enemy enemy)
	{
		AbilitySystem.OnAbilityHit += enemy.TryReceiveAbility;
		enemy.OnAbilityReceived += AbilitySystem.OnAbilityUsedHandler;
		enemy.OnDeath += OnDeathHandler;
	}

	private void CleanupEnemySignals(Enemy enemy)
	{
		AbilitySystem.OnAbilityHit -= enemy.TryReceiveAbility;
		enemy.OnAbilityReceived -= AbilitySystem.OnAbilityUsedHandler;
		enemy.OnDeath -= OnDeathHandler;
	}

	private Vector3 GetRandomPointAround(Vector3 center, float radius)
	{
		float angle = _rng.RandfRange(0.0f, Mathf.Tau);
		float distance = _rng.RandfRange(1.0f, radius);

		return center + new Vector3(
			Mathf.Cos(angle) * distance,
			0,
			Mathf.Sin(angle) * distance
		);
	}
}
