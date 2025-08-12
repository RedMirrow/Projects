using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made with Unity 2022.3.36f1 in mind
/// This code is for player controls using Input Manager Axes inputs
/// For flight movement in Unity 3D
/// 3 additonal axes were made - "Hover", "Boost" and "Roll"
/// To supplement the in-air movement
/// </summary>
public class ShipControl : MonoBehaviour
{
    [SerializeField]
    private PlayerHeat heatSystem;
    // Speed variables
    // Adjustable within the Script element in Unity
    public float forwardSpeed=40f;
    public float speedLimit = 100f;
    public float sideSpeed=40f;
    public float hoverSpeed=15f;
    public float boundsPunish = -0.1f; //The damage dealth to the player when they go out of bounds, grows more the longer the player is out of bounds
    public Transform playerLocation;
    public PlayerHealth playerHealth;
    private float activeForwardSpeed;
    private float activeSideSpeed;
    private float activeHoverSpeed;
    private float forwardAcceleration = 4f;
    private float sideAcceleration = 6f;
    private float hoverAcceleration = 2.5f;

    // Boost variables
    public float boostRate = 1.2f;
    public float boostLimit = 80f;
    public float boostHeatRate = 0.3f; // Heat build up

    public bool boosting = false; // Should boost the player if true
    

    // Rotation variables
    public float lookRate = 90f;
    private Vector3 lookInput, screenCentre, mouseDistance;
    // Start is called before the first frame update
    void Start()
    {
        screenCentre.x = Screen.width * 0.5f;
        screenCentre.y = Screen.height * 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Start dealing damage to the player should they go out of bounds of the play area
        if (playerLocation.position.x > 3200 || playerLocation.position.x < -3200 ||
            playerLocation.position.y > 3200 || playerLocation.position.y < -3200 ||
            playerLocation.position.z > 3200 || playerLocation.position.z < -3200)
        {
            FindObjectOfType<AudioManager>().Play("BoundsAlert");
            playerHealth.ChangeHealth(boundsPunish);
            boundsPunish -= 0.002f;
        }
        else { boundsPunish = -0.1f; } // Resetting the penalty

        if (heatSystem.currentHeat >= heatSystem.heatThreshold)
        { heatSystem.overHeat = true; boosting = false; }

        if (Input.GetAxisRaw("SkillMenu") == 0) {
            // Captures the mouse cursor position each update frame
            lookInput.x = Input.mousePosition.x;
            lookInput.y = Input.mousePosition.y;

            // Captures the distance of the cursor from the centre of the screen
            mouseDistance.x = (lookInput.x - screenCentre.x) / screenCentre.y;
            mouseDistance.y = (lookInput.y - screenCentre.y) / screenCentre.y;

            // Hardlimits the player model spin/rotation from mouse input
            // Without the hardlimit
            mouseDistance = Vector3.ClampMagnitude(mouseDistance, 1f);

            // If Q or E is pressed - roll to the left or right respectively
            if (Input.GetAxisRaw("Roll") != 0)
            {
                // Roll Left
                if (Input.GetAxisRaw("Roll") > 0)
                {
                    transform.Rotate(-mouseDistance.y * lookRate * Time.deltaTime,
                        mouseDistance.x * lookRate * Time.deltaTime,
                        -3.5f, Space.Self);
                }
                // Roll Right
                else
                {
                    transform.Rotate(-mouseDistance.y * lookRate * Time.deltaTime,
                    mouseDistance.x * lookRate * Time.deltaTime, 3.5f,
                    Space.Self);
                }
            }
            // If neither Q or E is pressed - do not apply the roll (0f)
            // Only turn the given model towards the direction of the cursor
            else
            {
                transform.Rotate(-mouseDistance.y * lookRate * Time.deltaTime,
                mouseDistance.x * lookRate * Time.deltaTime, 0f, Space.Self);
            }


            // Movement

            // Forward Movement
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,
                Input.GetAxisRaw("Vertical") * forwardSpeed,
                forwardAcceleration * Time.deltaTime);
            if (activeForwardSpeed > speedLimit && boosting == false) { activeForwardSpeed = speedLimit; }
            if (activeForwardSpeed > boostLimit && boosting == true) { activeForwardSpeed = boostLimit; }
            if (activeForwardSpeed < -20) { activeForwardSpeed = -20; }

            // Side Movement
            activeSideSpeed = Mathf.Lerp(activeSideSpeed,
                Input.GetAxisRaw("Horizontal") * sideSpeed,
                sideAcceleration * Time.deltaTime);

            // Hover - Up and Down Movement
            activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,
                Input.GetAxisRaw("Hover") * hoverSpeed,
                hoverAcceleration * Time.deltaTime);

            //Boosting
            if (Input.GetAxisRaw("Boost") != 0 && !heatSystem.overHeat && !heatSystem.blocking)
            {
                boosting = true;
                if (Input.GetAxisRaw("Boost") > 0)
                {
                    // Forward movement
                    if (activeForwardSpeed > boostLimit * boostRate)
                    {
                        activeForwardSpeed = boostLimit * boostRate;

                    }
                    heatSystem.HeatBoost(boostHeatRate);
                    // Prevents from going backwards too fast
                    if (activeForwardSpeed < -20f * boostRate) { activeForwardSpeed = -20 * boostRate; }
                    else { activeForwardSpeed = activeForwardSpeed * boostRate; }

                }
            }


            else { boosting = false; heatSystem.Cool(); }

            // Applies the movement transform forwards
            transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;

            // Applies the side and up/down movement
            transform.position += (transform.right * activeSideSpeed * Time.deltaTime) +
                (transform.up * activeHoverSpeed * Time.deltaTime);
        }

        

    }
    
    


}
