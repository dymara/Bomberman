using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using Assets.Scripts.Util;
using System.Collections.Generic;

public class RandomAI : MonoBehaviour {
	
	Board board;
	
	PositionConverter positionConverter;
	
	private bool isMoving;

	// Use this for initialization
	void Start () {
		board = GameObject.Find(Constants.GAME_MANAGER_NAME)
			.GetComponent<GameManager>().GetBoard();
		positionConverter = GameObject.Find(Constants.GAME_MANAGER_NAME)
			.GetComponent<GameManager>().GetPositionConverter();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isMoving) {
			isMoving = true;
			Move();
		}
	}
	
	void Move() 
	{
		Vector2 currentPosition = positionConverter.ConvertScenePositionToBoard(this.transform.position);
		List<GameCell> adjacentCells = board.GetAdjacentCells(currentPosition);
		GameCell nextCell = adjacentCells[Random.Range(0, adjacentCells.Count)];
		MakeMove(nextCell);
	}
	
	void MakeMove(GameCell targetCell) 
	{
		iTween.MoveTo(this.gameObject, iTween.Hash(
			"position", positionConverter.ConvertBoardPositionToScene(targetCell.GetCoordinates(), true),
			"oncomplete", "Move",
			"time", 3,
			"easetype", "linear"));	
	}
}
