using System;
using System.Linq;

namespace Sandbox
{
	public partial class MakeASnowmanCamera : Camera
	{
		public override void Activated()
		{
			Position = new Vector3();
			Rotation = Rotation.From( new Angles( 90, 0, 0 ) );

			base.Activated();
		}

		public override void Update()
		{
			if ( Local.Pawn is MakeASnowmanPlayer player )
			{
				var ball = (Game.Current as MakeASnowmanGame).LastSnowBall;
				var pos = player.Position + player.Rotation.Backward * 2000;

				if ( ball.IsValid() )
					pos = ball.Position + player.Rotation.Backward * 2000 + player.Rotation.Up * 500;

				FieldOfView = 80;
				Position = Position.LerpTo( pos, Time.Delta * 5 );
				Rotation = player.Rotation;
			}

			Viewer = null;
		}
	}
}
