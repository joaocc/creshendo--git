namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// Represents an abstract geometric primitive for
	/// use in the visualiser
	/// 
	/// </author>
	public abstract class Primitive
	{
		
		public Primitive()
		{
		}
		
		public abstract void  draw(Graphics2D canvas, int offsetX, int offsetY);
		public abstract void  draw(Graphics2D canvas, double factorX, double factorY);
		public abstract void  draw(Graphics2D canvas, int offsetX, int offsetY, double factorX, double factorY);
	}
}