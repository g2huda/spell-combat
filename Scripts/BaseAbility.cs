using Godot;

public partial class BaseAbility : Area3D
{

	[Signal] public delegate void OnAbilityHitEventHandler(Node3D target, float power);

	[Export] protected float Damage = 10.0f;

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
   
	public virtual void ActivateAbility(Node3D ownerActor, Vector3 direction, Vector3 atLocation)
	{
		GlobalPosition = atLocation;
		Show();
	}

	public void DeactivateAbility()
	{
		Hide();
	}

	protected virtual void OnBodyEntered(Node3D body)
	{
		EmitSignal(SignalName.OnAbilityHit, body, Damage);
	}
}
