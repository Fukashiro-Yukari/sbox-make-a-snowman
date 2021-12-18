using Sandbox;
using Sandbox.UI;

public partial class MakeASnowmanHud : HudEntity<RootPanel>
{
	public MakeASnowmanHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet.Load( "/ui/hud.scss" );

		RootPanel.AddChild<Score>();
		RootPanel.AddChild<GameOver>();
	}
}
