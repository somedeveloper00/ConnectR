using System;

public class WinCheck : IDisposable
{
	private int r;
	private Cell cell;
	private bool firstTime = true;

	public bool won { get; private set; }

	public WinCheck(int r) => this.r = r;

	public void Sample(Cell cell)
	{
		if (won) return;
		
		// first time check
		if (firstTime)
		{
			firstTime = false;
			this.cell = cell;
			r--;
			return;
		}

		if (this.cell == cell)
		{
			r--;
			if (r == 0)
				won = true;
		}
		
	}

	public void Dispose() { }
}
