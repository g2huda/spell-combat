using Godot;

public partial class GameCanvasLayer : CanvasLayer
{
	[Export] public Control HUD;

	public static GameCanvasLayer Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(Instance == null)
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
		if(Instance == this)
			Instance = null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
