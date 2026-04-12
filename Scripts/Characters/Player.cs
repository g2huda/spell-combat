using Godot;

public partial class Player : CharacterBody3D
{
	public const float JumpVelocity = 4.5f;

	[Export] protected InputHandler InputHandler;
	[Export] protected float MoveSpeed = 6f;
	[Export] protected float RotationSpeed = 2.5f;

	private float _moveInput;
	private float _turnInput;

	public  override void _Ready()
	{
		InputHandler.MoveInput += OnMoveInput;
		InputHandler.TurnInput += OnTurnInput;

		base._Ready();
	}

	public override void _ExitTree()
	{
		InputHandler.MoveInput -= OnMoveInput;
		InputHandler.TurnInput -= OnTurnInput;
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
}
