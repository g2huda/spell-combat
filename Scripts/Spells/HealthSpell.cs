using Godot;

public partial class HealthSpell : BaseAbility
{

	[Export] public float Speed = 12f;
	[Export] public float TravelDistance = 20f;

	private Tween _moveTween;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _ExitTree()
	{
		_moveTween?.Kill();
		base._ExitTree();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void FireAbility(Node3D ownerActor, Vector3 direction, Vector3 atLocation)
	{
		base.FireAbility(ownerActor, direction, atLocation);
		LookAt(ownerActor.GlobalPosition + direction.Normalized(), Vector3.Up);

		float travelTime = TravelDistance / Mathf.Max(Speed, 0.01f);
		Vector3 targetPosition = GlobalPosition + direction.Normalized() * TravelDistance;

		_moveTween?.Kill();
		_moveTween = CreateTween();
		_moveTween.TweenProperty(this, "global_position", targetPosition, travelTime);

		_moveTween.Finished += OnTravelFinished;
	}

	protected override void OnAbilityStoppedHandler()
	{
		base.OnAbilityStoppedHandler();
		_moveTween?.Kill();
	}

	private void OnTravelFinished() => EnableAbility(false);
}
