using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//need to INHERIT from CoreFeatures (not monobehaviour)
public class DoorFeatures : CoreFeatures
{
    /* --Door Features Map--
     * Door Pivot - Transform
     * Max Angle - Float
     * Reverse Direction - Bool
     * Speed - Float
     * Kinematics - Bool
     * Toggle Open/Close - Bool
     */

    //Components + Configurations
    [Header("Door Configurations")]
    [SerializeField]
    private Transform doorPivot; //pivot point for door rotation

    [SerializeField]
    private float maxAngle = 90.0f; //- probably will need to be less than 90 degrees

    [SerializeField]
    private bool reverseAngleDirection = false; //flips door open direction

    [SerializeField] 
    private float doorSpeed = 2.0f; //how fast door opens

    [SerializeField]
    private bool open = false; //is door open or closed

    [SerializeField]
    private bool MakeKinematicOnOpen = true; //makes door kinematic when open

    [Header("Interactions Configurations")]
    [SerializeField]
    private XRSocketInteractor socketInteractor; //socket interactor for door interaction

    [SerializeField] XRSimpleInteractable simpleInteractable; //"simple" interactable for door interaction, looks for some/any interaction

    
    private void Start()
    {
        //when key gets close to socket, add a listener
        //- s = Shorthand --> SelectEnterEvents
        //- "?" asks if socketInteractor exists, "=>" is lambda expression, ABSRACTION = hiding complexity
        socketInteractor?.selectEntered.AddListener((s) => 
        {
            //OpenDoor();
        });

        //when key leaves socket, add a listener
        socketInteractor?.selectExited.AddListener((s) => 
        {
            PlayOnEnd();
            socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true; //reusability, if used once = disable socket after use
        });

        //doors with simple interactors may not need key. Also good for cabinets, drawers, etc.
        simpleInteractable?.selectEntered.AddListener((s) =>
        {
            //OpenDoor();
        });
    }

    public void OpenDoor()
    {
        if (!open) //if door is not open...
        {
            PlayOnStart(); //...play on start
            open = true; //...set door open to true

            StartCoroutine(ProcessMotion());
        }
    }

    private IEnumerator ProcessMotion() 
    {
        //keep checking whether door is open or not
        while(open)
        {
            var angle = doorPivot.localEulerAngles.y < 180 ? doorPivot.localEulerAngles.y :
                doorPivot.localEulerAngles.y - 360;

            angle = reverseAngleDirection ? Mathf.Abs(angle) : angle;

            if (angle <= maxAngle)
            {
                doorPivot?.Rotate(Vector3.up, angle, doorSpeed * Time.deltaTime * (reverseAngleDirection) ? -1 : 1));
            }

            else
            {
                //when done opening, turn off rigidbody
                open = false;
                var featureRigidBody = GetComponent<Rigidbody>();
                if (featureRigidBody != null && MakeKinematicOnOpen) featureRigidBody.isKinematic = true;
                {

                }
            }

                yield return null;
        }
    }
}
