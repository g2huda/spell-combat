using Godot;

public partial class GameUi : Control
{
	[Export] protected Label SpellEquippedLabel;
	[Export] protected Label EnemiesRemainingLabel;

	public void UpdateSpellEquipped(StringName spellName)
	{
		SpellEquippedLabel.Text = $"{spellName.ToString()} equipped";
	}

	public void UpdateEnemiesRemaining(int count)
	{
		EnemiesRemainingLabel.Text = $"{count} enemies remaining";
	}
}
