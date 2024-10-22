using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    GridData floorData, furnitureData;


    [SerializeField]
    PreviewSystem preview;

    Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    ObjectPlacer objectPlacer;

    IBuildingState buildingState;
    private void Start()
    {
        StopPlacement();
        floorData = new ();
        furnitureData = new();
        //gridVisualization.SetActive(false);
        //floorData = new();
        //furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
     buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        //StopPlacement();
        //gridVisualization.SetActive(true);
        //buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
        //inputManager.OnClicked += PlaceStructure;
        //inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }
    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;

        //soundFeedback.PlaySound(SoundType.Click);
        //if (buildingState == null)
        //    return;
        //gridVisualization.SetActive(false);
        //buildingState.EndState();
        //inputManager.OnClicked -= PlaceStructure;
        //inputManager.OnExit -= StopPlacement;
        //lastDetectedPosition = Vector3Int.zero;
        //buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
             buildingState.UpdateState(gridPosition);   
            lastDetectedPosition = gridPosition;
        }

        //if (buildingState == null)
        //    return;
        //Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //if (lastDetectedPosition != gridPosition)
        //{
        //    buildingState.UpdateState(gridPosition);
        //    lastDetectedPosition = gridPosition;
        //}

    }
}
