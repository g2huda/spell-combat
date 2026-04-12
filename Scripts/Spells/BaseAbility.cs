using Godot;

public partial class BaseAbility : Area3D
{

	[Signal] public delegate void OnAbilityHitEventHandler(AbilityHitData hitData);

	[Export] protected float Damage = 10.0f;

	[Export] protected CollisionShape3D CollisionShape;

	[Export] protected PackedScene HitSfxScene;
	[Export] protected PackedScene HitVfxScene;

	protected virtual string AbilityID => GetType().Name;

	private Vector3 _currentDir;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		base._Ready();
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
		base._ExitTree();
	}
   
	public virtual void FireAbility(Node3D ownerActor, Vector3 direction, Vector3 atLocation)
	{
		GlobalPosition = atLocation;
		_currentDir = direction;
		EnableAbility(true);
	}

	public void EnableAbility(bool isActive)
	{
		Visible = isActive;
		CollisionShape.SetDeferred(CollisionShape3D.PropertyName.Disabled, !isActive);
		if (!isActive)
		{
			OnAbilityStoppedHandler();
		}
	}

	protected virtual void OnAbilityStoppedHandler()
	{
	}

	protected virtual void OnBodyEntered(Node3D body)
	{
		AbilityHitData hitData = GetHitData(body);
		EmitSignal(SignalName.OnAbilityHit, hitData);
	}

	private AbilityHitData GetHitData(Node3D target)
	{
		return new AbilityHitData
		{
			AbilityID = AbilityID,
			Target = target,
			HitPosition = GlobalPosition,
			HitNormal = -_currentDir,
			Power = Damage,
			SourceAbility = this,
			HitSfxScene = HitSfxScene,
			HitVfxScene = HitVfxScene
		};
	}
}
