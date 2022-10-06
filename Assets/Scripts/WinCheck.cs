using System;

public class WinCheck : IDisposable
{
	private int r;
	private Cell cell;

	public bool won { get; private set; }

	public WinCheck(int r) => this.r = r;

	public void Sample(Cell cell)
	{
		if (won) return;
		if (this.cell == cell)
			if (--r == 0)
				won = true;
	}

	public void Dispose() { }
}
