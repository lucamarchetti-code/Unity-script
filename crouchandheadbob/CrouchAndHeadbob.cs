using UnityEngine;

public class CrouchAndHeadbob : MonoBehaviour
{
    //crouch button
    public KeyCode key = KeyCode.LeftControl;

    //in the unity interface add here: transform of crouching first person controler, transform of first person camera.
    [Header("Low Head")]
    public Transform headToLower;
    public Transform cameraTransform;
    [HideInInspector]
    public float? defaultHeadYLocalPosition;
    public float crouchYHeadPosition = 1;

    //in the unity interface add here: collider of first person controller.
    public CapsuleCollider colliderToLower;
    [HideInInspector]
    public float? defaultColliderHeight;

    //vertical headbob variables. Change them as you see fit.
    [Header("Headbob")]
    [SerializeField] private float walkingBobSpeed = 14f;
    [SerializeField] private float walkingBobAmount = 0.05f;
    [SerializeField] private float runningBobSpeed = 18f;
    [SerializeField] private float runningBobAmount = 0.1f;
    [SerializeField] private float crouchingBobSpeed = 8f;
    [SerializeField] private float crouchingBobAmount = 0.025f;
    [SerializeField] private bool allowHeadbob = true;

    //horizontal headbob variables. Change them as you see fit.
    [Header("Headbob-sideways")]
    [SerializeField] private float walkingBobSpeedx = 8f;
    [SerializeField] private float walkingBobAmountx = 0.007f;
    [SerializeField] private float runningBobSpeedx = 12f;
    [SerializeField] private float runningBobAmountx = 0.01f;
    [SerializeField] private float crouchingBobSpeedx = 10f;
    [SerializeField] private float crouchingBobAmountx = 0.004f;

    private float defaultYPos = 0;
    [Range(10f, 100f)]
    public float Smooth = 10.0f;
    private bool IsRunning = false;
    public bool IsCrouched { get; private set; }

    void Awake()
    {
        // Try to get components.
        headToLower = //Transform of the head of your first person Gameobject (most of the time your Camera) that has to be lowered for the crouch
        cameraTransform = //Transform of your Camera
        colliderToLower = //Capsule colider of your first person Gameobject
    }

    void LateUpdate()
    {
        defaultYPos = headToLower.localPosition.y;

        if (Input.GetKey(key))
        {
            // Enforce a low head.
            if (headToLower)
            {
                // If we don't have the defaultHeadYLocalPosition, get it now.
                if (!defaultHeadYLocalPosition.HasValue)
                {
                    defaultHeadYLocalPosition = headToLower.localPosition.y;
                }

                // Lower the head.
                headToLower.localPosition = new Vector3(headToLower.localPosition.x, crouchYHeadPosition, headToLower.localPosition.z);
                defaultYPos = headToLower.localPosition.y;
            }

            // Enforce a low colliderToLower.
            if (colliderToLower)
            {
                // If we don't have the defaultColliderHeight, get it now.
                if (!defaultColliderHeight.HasValue)
                {
                    defaultColliderHeight = colliderToLower.height;
                }

                // Get lowering amount.
                float loweringAmount;
                if (defaultHeadYLocalPosition.HasValue)
                {
                    loweringAmount = defaultHeadYLocalPosition.Value - crouchYHeadPosition;
                }
                else
                {
                    loweringAmount = defaultColliderHeight.Value * .5f;
                }

                // Lower the colliderToLower.
                colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            // Set IsCrouched state.
            if (!IsCrouched)
            {
                IsCrouched = true;
                SetSpeedOverrideActive(true);
            }
        }
        else
        {
            if (IsCrouched)
            {
                // Rise the head back up.
                if (headToLower)
                {
                    headToLower.localPosition = new Vector3(headToLower.localPosition.x, defaultHeadYLocalPosition.Value, headToLower.localPosition.z);
                    defaultYPos = headToLower.localPosition.y;
                }

                // Reset the colliderToLower's height.
                if (colliderToLower)
                {
                    colliderToLower.height = defaultColliderHeight.Value;
                    colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
                }

                // Reset IsCrouched.
                IsCrouched = false;
            }
        }
        if (allowHeadbob)
        {
            headbob();
        }
    }


    void headbob()
    {

        //optional check to only toggle headbob when walking. Just add a bool that determines whether your first person is moving from your movement script into the if condition
        //if the first person is not movin the camera is moved back into its original position
        //if(check if first person is walking){

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * (IsCrouched ? crouchingBobSpeed : IsRunning ? runningBobSpeed : walkingBobSpeed)) * (IsCrouched ? crouchingBobAmount : IsRunning ? runningBobAmount : walkingBobAmount) * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * (IsCrouched ? crouchingBobSpeedx : IsRunning ? runningBobSpeedx : walkingBobSpeedx) / 2f) * (IsCrouched ? crouchingBobAmountx : IsRunning ? runningBobAmountx : walkingBobAmountx) * 1.6f, Smooth * Time.deltaTime);
        cameraTransform.localPosition += pos;

        //optional else
        /*}else{
            if (cameraTransform.localPosition == new Vector3(0,0,0)) return;
            cameraTransform.localPosition = Vector3.Lerp(
                    cameraTransform.localPosition,
                    new Vector3(0,0,0),
                    2*Time.deltaTime
                    );
        }*/
    }

    public void toggleHeadbob()
    {
        if (allowHeadbob)
        {
            allowHeadbob = false;
        }
        else
        {
            allowHeadbob = true;
        }
    }