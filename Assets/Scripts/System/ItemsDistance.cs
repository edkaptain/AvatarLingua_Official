using System.Collections.Generic;
using UnityEngine;

public class ItemsDistance : MonoBehaviour
{
    [Header("Items")]
    public GameObject itemsParent;
    public List<GameObject> items = new List<GameObject>();

    [Header("Distance")]
    public float distanceThreshold = 3f;
    public Material greenMaterial;
    public Material redMaterial;


    [SerializeField]
    private List<LineRenderer> connectionLines = new List<LineRenderer>();
    public List<GameObject> connections = new List<GameObject>();

    // Testing
    [ContextMenu("Nodes")]
    public void UpdateNodesLineRenderer()
    {
        connections.Clear();
        foreach (Transform child in itemsParent.transform)
        {
            connections.Add(child.gameObject);
        }

        for (int i = 0; i < connections.Count; i++)
        {
            for (int j = i + 1; j < connections.Count; j++)
            {
                float distance = Vector3.Distance(connections[i].transform.position, connections[j].transform.position);

                Debug.LogWarning($"{connections[i].name} -> {connections[j].name}: {distance}");
            }
        }

    }

    /// <summary>
    /// Adds all direct children of itemsParent to the items list.
    /// </summary>
    [ContextMenu("Add Items to List")]
    public void UpdateLineRenderer()
    {
        items.Clear();

        if (itemsParent == null)
        {
            Debug.LogWarning("Items Parent has not been assigned.");
            return;
        }

        foreach (Transform child in itemsParent.transform)
        {
            items.Add(child.gameObject);
        }
    }

    /// <summary>
    /// Creates lines between consecutive items and changes their
    /// materials according to the distance between them.
    /// </summary>
    [ContextMenu("Show Line Renderers")]
    public void ShowLineRender()
    {
        UpdateLineRenderer();


        if (items.Count < 2)
        {
            Debug.LogWarning(
                "At least two items are required."
            );

            DisableUnusedLines(0);
            return;
        }

        int requiredLines = items.Count - 1;

        for (int i = 0; i < requiredLines; i++)
        {
            LineRenderer currentLine = GetOrCreateLine(i);

            Vector3 firstPoint = items[i].transform.position;

            Vector3 secondPoint = items[i + 1].transform.position;

            float distance = Vector3.Distance(firstPoint, secondPoint);

            currentLine.enabled = true;
            currentLine.useWorldSpace = true;
            currentLine.positionCount = 2;

            currentLine.startWidth = 0.1f;

            currentLine.endWidth = 0.01f;

            currentLine.SetPosition(0, firstPoint);
            currentLine.SetPosition(1, secondPoint);

            if (distance > distanceThreshold)
            {
                currentLine.sharedMaterial = greenMaterial;
            }
            else
            {
                currentLine.sharedMaterial = redMaterial;
            }

            Debug.Log($"Distance {i + 1}-{i + 2}: {distance}");
        }

        DisableUnusedLines(requiredLines);
    }

    /// <summary>
    /// Returns an existing LineRenderer or creates a new one.
    /// </summary>
    private LineRenderer GetOrCreateLine(int index)
    {
        // If index is less than list and the list index is different than null
        if (index < connectionLines.Count && connectionLines[index] != null)
        {
            return connectionLines[index];
        }

        // Creates a new object
        GameObject lineObject = new GameObject($"Line {index + 1}-{index + 2}");
        // Set the parent
        lineObject.transform.SetParent(transform);
        // Creates a new lineRender 
        LineRenderer newLine = lineObject.AddComponent<LineRenderer>();

        // If the current index is less than the list length, the current list position will be this line, else it will create a new one if there is no't a line actaully.
        if (index < connectionLines.Count)
        {
            connectionLines[index] = newLine;
        }
        else
        {
            connectionLines.Add(newLine);
        }

        return newLine;
    }

    /// <summary>
    /// Disables LineRenderers that are no longer required.
    /// </summary>
    private void DisableUnusedLines(int requiredLines)
    {
        for (int i = requiredLines;
             i < connectionLines.Count;
             i++)
        {
            if (connectionLines[i] != null)
            {
                connectionLines[i].enabled = false;
            }
        }
    }


}