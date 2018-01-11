using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

	static int correct=0;//C
	static int wrong=0;//E
	static int score=0;//final score in %
	public static int halfScore = 0;
	public static int fullScore = 0;

	public static bool countdown = false;
	public static bool hasPlayed = false;
	public static  AudioSource audio1;

	public static int points;
	public static int totalPoints;

	void Awake()
	{
		AudioSource[] sounds = gameObject.GetComponents<AudioSource>();
		audio1 = sounds[0];
	}

	//Correct Answerss
	public static int GetCorrect()
	{
		return correct;
	}

	public static void SetCorrect(int n_correct)
	{
		if (n_correct == 0)
		{
			correct = 0;
		}
		else
		{
			correct = correct + n_correct;
			//if it's a correct answer coming from a hint, only gets 50% of the score
			if (Hints.hintActive)
			{
				halfScore ++;
			}
			else
			{
				fullScore ++;
			}
		}
	}
	
	public static void DeleteCorrect(int del_correct){correct = correct-del_correct;}
		
	//Error Answers
	public static int GetError(){return wrong;}
	public static void SetError(int n_error)
	{
		if (n_error == 0)
		{
			wrong = 0;
		}
		else
		{
			wrong = wrong + n_error;
		}
	}
	
	public static void DeleteError(int del_error){wrong = wrong-del_error;}
		
	//Final score
	public static int GetScore()
	{
		if (SpawnTiles.levelName != "L0")
			score = ((fullScore * 10) + (halfScore * 5) - (wrong * 10)) / SpawnTiles.correctchoices;
		else
			score = 0;
		return score; 
	}

   public static int GetPoints()
	{
		if (SpawnTiles.levelName != "L0")
			points = (fullScore * 10) + (halfScore * 5);
		else
			points = 0;
		return points;
	}

	public static void SetTotalPoints(int points)
	{
		 totalPoints = totalPoints + points;
	}

	public static int GetTotalPoints()
	{
		return totalPoints;
	}

	void Update()
	{
		if (SpawnTiles.timer > 0 && MainGUI.timeToDisplay <= 4.5f && SpawnTiles.imagesLoaded)
		{
			
			if (Main_Menu.sound)
			{
				if (audio1.isPlaying)
				{
					hasPlayed = true;
				}
				else
				{
					PlaySound();
				}
			}
		}
	}

	void PlaySound()
	{
		if (!hasPlayed)
		{
			audio1.Play();
		}
	}
}
