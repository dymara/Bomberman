using Assets.Scripts.Board;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlighter : MonoBehaviour {

    public Component cellHighlightPrefeb;

    private float cubeSize;

    private Queue<Component> unusedHighlights;

    private Queue<Component> highlightsInUse;

    private HashSet<GameCell> highlightedCells;

    private readonly object lockObject = new object();

    public void Awake()
    {
        this.cubeSize = GameManager.instance.GetCellSize();
        this.cellHighlightPrefeb.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        this.unusedHighlights = new Queue<Component>();
        this.highlightsInUse = new Queue<Component>();
        this.highlightedCells = new HashSet<GameCell>();
    }

    public void EnableCellsHighlight(HashSet<GameCell> cellsToExplode)
    {
        lock (lockObject)
        {
            foreach (GameCell cell in cellsToExplode) {
                EnableCellHighlight(cell);
            }
        }
    }

    public void DisableCellsHighlights()
    {
        lock (lockObject)
        {
            while (highlightsInUse.Count > 0)
            {
                Component highlight = highlightsInUse.Dequeue();
                highlight.gameObject.SetActive(false);
                unusedHighlights.Enqueue(highlight);
            }
            highlightedCells.Clear();
        }
    }

    private void EnableCellHighlight(GameCell cell)
    {
        if (!highlightedCells.Contains(cell))
        {
            if (unusedHighlights.Count == 0)
            {
                CreateCellHighlight();
            }
            Component highlight = unusedHighlights.Dequeue();
            float posX = (cell.GetCoordinates().x * cubeSize) + cubeSize / 2;
            float posZ = (cell.GetCoordinates().y * cubeSize) + cubeSize / 2;
            highlight.transform.localPosition = new Vector3(posX, 0.01f, posZ);
            highlight.gameObject.SetActive(true);
            highlightsInUse.Enqueue(highlight);
            highlightedCells.Add(cell);
        }
    }

    private void CreateCellHighlight()
    {
        Component highlight = Instantiate(cellHighlightPrefeb) as Component;
        highlight.name = "Cell Highlight #" + (highlightsInUse.Count + unusedHighlights.Count + 1);
        highlight.transform.parent = transform;
        highlight.gameObject.SetActive(false);
        unusedHighlights.Enqueue(highlight);
    }

}
