using System;
using System.Reflection;

namespace JulianSchoenbaechler.GameState
{
	public class CustomGameState : Attribute
	{
		public override string ToString()
		{
			return string.Format("[CustomGameState]");
		}
	}
}
