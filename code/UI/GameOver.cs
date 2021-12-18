using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class GameOver : Panel
{
	Label GameOverTitle;
	Label GameOverScore;

	public GameOver()
	{
		var GameOverPanel = Add.Panel( "Background" );
		GameOverTitle = GameOverPanel.Add.Label( "Game Over", "Title" );
		GameOverScore = GameOverPanel.Add.Label( "0", "Score" );
	}

	public override void Tick()
	{
		var game = Game.Current as MakeASnowmanGame;

		SetClass( "IsNotGameOver", !game.IsGameOver );

		GameOverScore.Text = $"Score: {game.GameOverScore}";
	}
}
