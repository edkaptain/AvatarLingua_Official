using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemsManager : MonoBehaviour
{
    public List<Items> items = new List<Items>();
    public int currentClicked = 0;

    public UnityEvent events;
    // Singleton
    public static ItemsManager Instance;
    public Door door;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        door.UpdateDoorCanvas();
    }

    public void AddToList(Items item)
    {
        items.Add(item);
    }


    /// <summary>
    ///  Adds one point to the global score
    /// </summary>
    public void AddScore()
    {
        currentClicked++;
        door.UpdateDoorCanvas();
        if (currentClicked == items.Count)
        {
            events?.Invoke();
        }
    }

    public int GetActualScore()
    {
        return (items.Count + 1) - currentClicked;
    }
    
}
