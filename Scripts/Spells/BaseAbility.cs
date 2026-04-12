using Godot;

public partial class BaseAbility : Area3D
{

	[Signal] public delegate void OnAbilityHitEventHandler(Node3D target, float power, BaseAbility ability);

	[Export] protected float Damage = 10.0f;

	[Export] protected CollisionShape3D CollisionShape;

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
		EnableAbility(true);
	}

	public void EnableAbility(bool isActive)
	{
		Visible = isActive;
		Monitoring = isActive;
		Monitorable = isActive;
		CollisionShape.SetDeferred(CollisionShape3D.PropertyName.Disabled, !isActive);
	}

	protected virtual void OnBodyEntered(Node3D body)
	{
		EmitSignal(SignalName.OnAbilityHit, body, Damage, this);
	}
}
