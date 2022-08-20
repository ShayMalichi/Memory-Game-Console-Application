using System;

public class Player
{
	private string m_Name;
	private int m_Score;
	private bool m_IsMyTurn;

	public Player()
	{
		this.m_Name = "";
		this.m_Score = 0;
		this.m_IsMyTurn = false;
	}

	public Player(string i_Name)
	{
		this.m_Name = i_Name;
		this.m_Score = 0;
		this.m_IsMyTurn = false;
	}

	public void SetName(String i_Name)
	{
		this.m_Name = i_Name;
	}

	public string GetName()
    {
		return this.m_Name;
    }

	public bool IsMyTurn()
	{
		return this.m_IsMyTurn;
	}

	public int GetScore()
	{
		return this.m_Score;
	}

	public void IncrementScore()
	{
		this.m_Score++;
	}

	public void SetTurn()
	{
		this.m_IsMyTurn = true;
	}

	public void EndTurn()
	{
		this.m_IsMyTurn = false;
	}
}

