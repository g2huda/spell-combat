using Godot;

public partial class Player : CharacterBody3D
{
	public const float JumpVelocity = 4.5f;

	//trigger event when player is ready to cast a spell, used to sync animation and ability execution
	[Signal] public delegate void OnSpellReadyEventHandler(Vector3 abilityPoint);
	[Signal] public delegate void SwitchAbilityReadyEventHandler();

	[Export] protected InputHandler InputHandler;
	[Export] protected PlayerAnimationController AnimationController;
	[Export] protected float MoveSpeed = 6f;
	[Export] protected float RotationSpeed = 2.5f;
	[Export] protected Marker3D AbilityPoint;

	private float _moveInput;
	private float _turnInput;

    public  override void _Ready()
	{
		InputHandler.MoveInput += OnMoveInput;
		InputHandler.TurnInput += OnTurnInput;
		InputHandler.MoveInput += AnimationController.SetMoveSpeed;
		InputHandler.AbilityRequested += AnimationController.TriggerAttack;
		InputHandler.SwitchAbility += OnSwitchAbilityHandler;//todo: can add animation for switching ability later
		AnimationController.OnSpellReady += OnSpellReadyHandler;
		base._Ready();
	}

	public override void _ExitTree()
	{
		InputHandler.MoveInput -= OnMoveInput;
		InputHandler.TurnInput -= OnTurnInput;
		InputHandler.MoveInput -= AnimationController.SetMoveSpeed;
		InputHandler.AbilityRequested -= AnimationController.TriggerAttack;
		InputHandler.SwitchAbility -= OnSwitchAbilityHandler;
		AnimationController.OnSpellReady -= OnSpellReadyHandler;
		base._ExitTree();
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;

		// Rotation
		RotateY(-_turnInput * RotationSpeed * deltaF);

		// Forward movement
		Vector3 forward = -Transform.Basis.Z;
		Velocity = forward * _moveInput * MoveSpeed;

		MoveAndSlide();
	}

	public void OnMoveInput(float value) => _moveInput = value;
	private void OnTurnInput(float value) => _turnInput = value;
	private void OnSpellReadyHandler() => EmitSignal(SignalName.OnSpellReady, AbilityPoint.GlobalPosition);
	private void OnSwitchAbilityHandler() => EmitSignal(SignalName.SwitchAbilityReady);

}
