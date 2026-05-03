using Godot;

public partial class MainMenu : Control
{
	[Export] private Button _startButton;
	[Export] private Button _quitButton;

	public override void _Ready()
	{
		_startButton.Pressed += OnStartPressed;
		_quitButton.Pressed += OnQuitPressed;
	}

	public override void _ExitTree()
	{
		_startButton.Pressed -= OnStartPressed;
		_quitButton.Pressed -= OnQuitPressed;
	}

	private void OnStartPressed()
	{
		LevelManager.Instance.LoadGame();
	}

	private void OnQuitPressed()
	{
		LevelManager.Instance.Quit();
	}
}
