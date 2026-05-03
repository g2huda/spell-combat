using Godot;

public partial class WinPopup : Control
{
	[Signal] public delegate void OnMainMenuButtonPressedEventHandler();

	[Export] private Button _mainMenuButton;
	private Tween _tween;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenuButton.Pressed += OnMainMenuButtonPressedHandler;
		_tween = CreateTween();
		Modulate = new Color(1, 1, 1, 0);
		_tween.TweenProperty(this, "modulate:a", 1.0f, 1.5f);
		base._Ready();
	}

	public override void _ExitTree()
	{
		_mainMenuButton.Pressed -= OnMainMenuButtonPressedHandler;
		_tween?.Kill();
		base._ExitTree();
	}

	private void OnMainMenuButtonPressedHandler()
	{
		EmitSignal(SignalName.OnMainMenuButtonPressed);
	}
}
