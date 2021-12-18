using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class MakeASnowmanGame : Game
{
	public static readonly float OffsetDown = 250;
	public static readonly float PlayerScale = 15;

	public static List<SnowBall> snowBalls = new();
	public static SnowBall lastSnowBall { get; set; }

	[Net] public bool IsGameOver { get; set; }
	[Net] public int GameOverScore { get; set; }

	public MakeASnowmanGame()
	{
		if ( IsServer )
			_ = new MakeASnowmanHud();
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		var player = new MakeASnowmanPlayer();
		player.ReSpawn();

		cl.Pawn = player;
	}

	public virtual void GameOver()
	{
		if ( IsGameOver ) return;

		IsGameOver = true;

		var score = 0;

		foreach ( var ball in snowBalls )
		{
			if ( ball.PhysicsBody.BodyType == PhysicsBodyType.Static )
				score++;
		}

		GameOverScore = score;

		_ = DoGameOver();
	}

	async Task DoGameOver()
	{
		await GameTask.DelaySeconds( 4 );

		foreach ( var ball in snowBalls )
		{
			if ( ball.IsValid() )
				ball.Delete();
		}

		snowBalls.Clear();

		foreach ( var ply in All.OfType<MakeASnowmanPlayer>() )
			ply.ReSpawn();

		IsGameOver = false;
	}
}
