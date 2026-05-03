using Godot;

public partial class LevelManager : Node
{
	[Export] private PackedScene _mainMenuScene;
	[Export] private PackedScene _gameScene;

	public static LevelManager Instance { get; private set; }

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	   
		else
		{
				GD.PushWarning("LevelManager: An instance already exists. This instance will be freed.");
				QueueFree();
		}
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}


	public void LoadMainMenu()
	{
		ChangeScene(_mainMenuScene);
	}

	public void LoadGame()
	{
		ChangeScene(_gameScene);
	}

	public void Quit()
	{
		GetTree().Quit();
	}

	private void ChangeScene(PackedScene scene)
	{
		if(scene == null)
		{
			GD.PushError("LevelManager: Scene is not assigned.");
			return;
		}

		GetTree().ChangeSceneToPacked(scene);
	}
}
