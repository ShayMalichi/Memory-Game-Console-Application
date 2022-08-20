using System;
using System.Collections.Generic;

public class Board
{
	
	private Card[,] m_CardsInGame;

	public Board(int i_Height, int i_Width)
	{
		this.m_CardsInGame = new Card[i_Height, i_Width];
	}

	public void InitBoard()
	{
		Random random = new Random();
		List<char> randomLetters = getRandomLetters(this.m_CardsInGame.GetLength(0), m_CardsInGame.GetLength(1));
		int randomHight = random.Next(0, this.m_CardsInGame.GetLength(0));
		int randomWidth = random.Next(0, this.m_CardsInGame.GetLength(1));

		while (randomLetters.Count > 0)
		{
			while (this.m_CardsInGame[randomHight, randomWidth] != null)
			{
				randomHight = random.Next(0, this.m_CardsInGame.GetLength(0));
				randomWidth = random.Next(0, this.m_CardsInGame.GetLength(1));
			}

			this.m_CardsInGame[randomHight, randomWidth] = new Card(randomLetters[0]);
			randomLetters.RemoveAt(0);
		}
	}

	public Card[,] GetCardsInGame()
	{
		return this.m_CardsInGame;
	}

	public bool IsAllCardsVisible()
	{
		bool v_AllVisible = true;

		for (int i = 0; i < this.m_CardsInGame.GetLength(0); i++)
		{
			for (int j = 0; j < this.m_CardsInGame.GetLength(0); j++)
			{
				v_AllVisible = v_AllVisible && this.m_CardsInGame[i, j].GetIsVisible();
			}
		}

		return v_AllVisible;
	}

	public void Close2Card(Card i_FirstCardPicked, Card i_SecondCardPicked)
	{
		i_FirstCardPicked.TurnCardInvisible();
		i_SecondCardPicked.TurnCardInvisible();
	}

	public void OpenCard(Card i_CardPicked)
	{
		i_CardPicked.TurnCardVisible();
	}

	public int GetBoardHight()
	{
		return this.m_CardsInGame.GetLength(0);
	}

	public int GetBoardWidth()
	{
		return this.m_CardsInGame.GetLength(1);
	}

	private static List<char> getRandomLetters(int i_NumOfRows, int i_NumOfColoms)
	{
		int numberOfLetters = (i_NumOfRows * i_NumOfColoms) / 2;
		List<char> setOfLetters = new List<char>(numberOfLetters);
		Random rand = new Random();
		char currentCharacter = (char)rand.Next(65, 90);
		
		while (setOfLetters.Count < numberOfLetters * 2)
		{
			while (setOfLetters.Contains(currentCharacter))
			{
				currentCharacter = (char)rand.Next(65, 90);
			}
			setOfLetters.Add(currentCharacter);
			setOfLetters.Add(currentCharacter);
		}

		return setOfLetters;
	}
}