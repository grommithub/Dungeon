using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _start, _corridor, _room;
    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        for(int i = 0; i < _start.transform.childCount; i++)
        {
            if(_start.transform.GetChild(i).CompareTag("DoorPosition"))
            {
                GameObject corridor = Instantiate(_corridor, _start.transform.GetChild(i).transform.position, _start.transform.GetChild(i).transform.rotation);
                
                for(int j = 0; j < corridor.transform.childCount; j++)
                {
                    if (corridor.transform.GetChild(j).CompareTag("DoorPosition"))
                    {
                        print("new room");
                        GameObject newRoom = Instantiate(_room, corridor.transform.GetChild(j).transform.position, corridor.transform.GetChild(j).transform.rotation);
                    }
                    else print("no new room");
                }
            }
        }
    }

    private void Update()
    {
        
    }
}
