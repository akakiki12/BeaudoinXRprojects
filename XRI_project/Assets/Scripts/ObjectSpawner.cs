using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR; //for communicating with quest controllers

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; //object to spawn
    public Transform spawnPoint; //where it spawns
    public XRNode controllerNode = XRNode.RightHand; //assigning controller
    public float spawnCooldown = 1.0f; //needs coroutine, spawn cooldown
    private bool canSpawn = true; //time in seconds between spawns



    // Update is called once per frame
    void Update()
    {
        if (canSpawn && IsAButtonPressed()) //checks if we can spawn and if button is pressed
        {
            StartCoroutine(SpawnObjectWithCooldown()); //starts coroutine to spawn object with cooldown
        }
    }

    //Checks if button is pressed
    bool IsAButtonPressed()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode); //gets device at assigned controller node
        bool buttonPressed = false; //false by default

        //tries to get button input from controller
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out buttonPressed) && buttonPressed) //reads primary button, stores value in buttonPressed && checks if button is pressed
        {
            return true; //returns true if button is pressed
        }

        return false; //returns false if button is not pressed
    }

    //Respawning and cooldown
    IEnumerator SpawnObjectWithCooldown()
    {
        canSpawn = false; //prevents immediate respawning
        SpawnObject(); 
        yield return new WaitForSeconds(spawnCooldown); //delay to respawn
        canSpawn = true; //allows us to spawn again

    }

    void SpawnObject()
    {
        if(objectPrefab != null && spawnPoint != null) //checks if objectPrefab and spawnPoint are assigned
        {
            GameObject spawnedObject  = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation); //spawns object at spawn point position and rotation
        }

        else
        {
            Debug.LogError("Assign objectPrefab and spawnPoint in the inspector!"); //error message if not assigned

        }
    }

}
