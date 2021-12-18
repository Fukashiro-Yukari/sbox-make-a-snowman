using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Score : Panel
{
	Label scoreLabel;

	public Score()
	{
		scoreLabel = Add.Label( "0", "ScoreText" );
	}

	public override void Tick()
	{
		var ply = Local.Pawn as MakeASnowmanPlayer;

		if ( ply == null ) return;

		scoreLabel.Text = $"Score: {ply.SnowBallCount}";
	}
}
