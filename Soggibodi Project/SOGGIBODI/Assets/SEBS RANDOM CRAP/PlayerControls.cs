using Clayxels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable] public enum ControllerState
{
    UPRIGHT,    // Behaving Regularly
    FLOPPED,    // Flopped out bro
    STUNNED,    // if hit with a heavy weapon/hammer or something? basically upright but unresponsive
    DEAD        // rest in piss wont be missed
}

[System.Serializable] public struct RagdollAnchor
{
    // For easier inspector view
    [SerializeField] string name;
    // For accessing each anchor's joint/rigidbody/etc.
    [SerializeField] public GameObject anchor;
    // these are used to determine which joints become rigid when extending arms: Elbows and Hands only, keep flexibility in shoulders
    [SerializeField] public bool isLeftArm, isRightArm; 
}

public class PlayerControls : MonoBehaviour
{
    [SerializeField] public Color playerColour;
    [Header("Input")]
    [SerializeField] PlayerInput playerInputs;
    private InputAction moveAction;
    private InputAction spinAction;
    private InputAction leftArmAction;
    private InputAction rightArmAction;
    [SerializeField] public ControllerState currentState{get; private set;}

    private InputAction flopAction;
    private InputAction jumpAction;

    public List<ClayObject> objects = new();

    [Header("Ragdoll Points")]
    [SerializeField] List<RagdollAnchor> ragdollAnchors = new List<RagdollAnchor>();
    [SerializeField] Rigidbody hips, lHand, rHand;
    [SerializeField] Transform lHandle, rHandle; // used for parenting when we get round to spawning weapons - maybe do a weapon select screen per-player?
    
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 currentMoveDir;
    [SerializeField] float spinSpeed;
    private float totalRotation = 0f;
    [SerializeField] float jumpForce;
    [SerializeField] bool isGrounded = false;

    [Header("Combat")]
    [SerializeField] public GameObject lWeapon;
    [SerializeField] public GameObject rWeapon; // which weapon is in each hand - this will be assigned in the character customisation screen and transferred between scenes
    [SerializeField] Weapon spawnL;
    [SerializeField] Weapon spawnR;
    [SerializeField] float maxHealth, currentHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] float damageCooldownCurrent, damageCooldownMax = 0.1f; // hopefully avoid dodgy double hits

    [Header("Flop")]

    [Tooltip("Time in ms before auto-unflop")]
    [SerializeField] private float flopTime = 3000.0f;

#region Unity Funcs
    void Awake()
    {
        currentState = ControllerState.UPRIGHT;

        playerInputs = GetComponentInParent<PlayerInput>();
        moveAction = playerInputs.actions["Move"];
        spinAction = playerInputs.actions["Spin"];
        leftArmAction = playerInputs.actions["ExtendLeft"];
        rightArmAction = playerInputs.actions["ExtendRight"];
        flopAction = playerInputs.actions["Flop"];
        jumpAction = playerInputs.actions["Jump"];

        currentHealth = maxHealth;
        damageCooldownCurrent = 0;
    }

    void OnEnable()
    {
        foreach(ClayObject c in objects)
        {
            c.color = playerColour;
        }

        GameObject L = Instantiate(lWeapon, lHandle);
        GameObject R = Instantiate(rWeapon, rHandle);

        L.GetComponentInChildren<FixedJoint>().connectedBody = lHand;
        R.GetComponentInChildren<FixedJoint>().connectedBody = rHand;

        spawnL = L.GetComponentInChildren<Weapon>();
        spawnR = R.GetComponentInChildren<Weapon>();


        moveAction.started += LockMovementDir;

        rightArmAction.started += ExtendRightArm;
        rightArmAction.performed += ExtendRightArm;
        rightArmAction.canceled += UnextendRightArm;

        leftArmAction.started += ExtendLeftArm;
        leftArmAction.performed += ExtendLeftArm;
        leftArmAction.canceled += UnextendLeftArm;

        flopAction.started += Flop;

        jumpAction.started += Jump;
    }

    void FixedUpdate()
    {
        float movement = moveAction.ReadValue<float>();
        float spin = spinAction.ReadValue<float>();
        float arm = leftArmAction.ReadValue<float>();

        if(currentState == ControllerState.UPRIGHT){
            hips.AddForce(currentMoveDir * movement * moveSpeed); // forward/backward movement

            IncrementQuaternionEulerY(hips.GetComponent<ConfigurableJoint>(), spin); // shpin
        }

        healthBar.fillAmount = currentHealth / maxHealth;
        if(damageCooldownCurrent > 0) {damageCooldownCurrent -= Time.deltaTime;}
    }

    void OnDisable()
    {
        moveAction.started -= LockMovementDir;

        rightArmAction.started -= ExtendRightArm;
        rightArmAction.performed -= ExtendRightArm;
        rightArmAction.canceled -= UnextendRightArm;

        leftArmAction.started -= ExtendLeftArm;
        leftArmAction.performed -= ExtendLeftArm;
        leftArmAction.canceled -= UnextendLeftArm;

        flopAction.started -= Flop;

        jumpAction.started -= Jump;
    }
#endregion

    void LockMovementDir(InputAction.CallbackContext context){
        currentMoveDir = hips.transform.forward;
    }

    async void Flop(InputAction.CallbackContext context)
    {
        currentState = ControllerState.FLOPPED;
        // Every Ragdoll Anchor has their Angular X Drive Position Spring and Angular YZ Drive Postion Spring
        // Set to 0
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 0.0f);
            }
        }

        // After a few seconds, Unflop()
        await AutoUnflop();
    }
    public async void Flop()
    {
        currentState = ControllerState.FLOPPED;
        // Every Ragdoll Anchor has their Angular X Drive Position Spring and Angular YZ Drive Postion Spring
        // Set to 0
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 0.0f);
            }
        }

        // After a few seconds, Unflop()
        await AutoUnflop();
    }

    private async Task AutoUnflop()
    {
        // Await Flop Time
        await Task.Delay(flopTime.ConvertTo<int>());
        Unflop();
    }

    void Unflop()
    {
        if(currentState == ControllerState.FLOPPED){
            currentState = ControllerState.UPRIGHT;
        }

        // Every Ragdoll Anchor has their Angular X Drive Position Spring and Angular YZ Drive Postion Spring
        // Set to 750
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 750.0f);
                // arms wanna still be floppy
                if(a.isLeftArm || a.isRightArm){
                    SetJointDrives(j, 0.0f);
                }
            }
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        switch(currentState){
            case ControllerState.FLOPPED:    // Unflop if currently flopped
                Unflop();
                break;

            case ControllerState.UPRIGHT:   // Jump if grounded
                if(isGrounded){
                    hips.AddForce(hips.transform.up * jumpForce);
                }
                break;

            default:                        // do nothing if stunned or dead
                break;
        }
    }


    /// <summary>
    /// Functions for extending and unextending a player's arms. When extended, the elbow and hand joints become rigid and stick out,
    /// but the shoulder remains floppy so when the player spins around, their arms flail wildly but remain in a straight line.
    /// Weapons are attached as children to the hands so they will flail around as well.
    /// </summary>
#region Arms Controls
    void ExtendRightArm(InputAction.CallbackContext context)
    {
        // SetArmAnchors(PLAYER_LIMPS.RIGHT_ARM, 750.0f);
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.isRightArm && a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 750.0f);
            }
        }
        if(rWeapon) {spawnR.OnActivate();}
    }

    void UnextendRightArm(InputAction.CallbackContext context)
    {
        // Take Ragdoll Anchors in Right Arm
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.isRightArm && a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 0.0f);
            }
        }
        if(rWeapon) {spawnR.OnDeactivate();}
        
    }

    void ExtendLeftArm(InputAction.CallbackContext context)
    {
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.isLeftArm && a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 1500.0f);
            }
        }
        if(lWeapon) {spawnL.OnActivate();}
    }

    void UnextendLeftArm(InputAction.CallbackContext context)
    {
        // SetArmAnchors(PLAYER_LIMPS.LEFT_ARM, 0f);
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.isLeftArm && a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 0.0f);
            }
        }
        if(lWeapon) {spawnL.OnDeactivate();}
    }
#endregion

#region Combat Functions

    public void TakeDamage(float _amount)
    {
        if(damageCooldownCurrent <= 0){
            Debug.Log(this.gameObject.name + " took " + _amount + " damage");

            currentHealth -= _amount;
            if(currentHealth <= 0){
                Die();
            }
        }
        AudioController.instance.PlayAudio(AudioType.SFX_02);
        damageCooldownCurrent = damageCooldownMax;
    }
    public void RestoreHealth(float _amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + _amount);
    }
    public void Die()
    {
        // do some stupid animation shit and fall to pieces or something
        foreach(RagdollAnchor a in ragdollAnchors){
            if(a.anchor.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint j)){
                SetJointDrives(j, 0.0f);
            }
        }
        //fukin die mad lols
        currentState = ControllerState.DEAD;
        GameManager gm = FindObjectOfType<GameManager>();
        gm.listOfPlayer.Remove(this);
        if(gm.listOfPlayer.Count <= 1)
        {
            SceneController.instance.Load(SceneType.End);
        }
    }

#endregion

#region Utils
    // Generic function, takes in a joint and changes those annoying Position Spring values
    private void SetJointDrives(ConfigurableJoint _joint, float _val)
    {
        JointDrive xDrive = _joint.angularXDrive;
        xDrive.positionSpring = _val;
        _joint.angularXDrive = xDrive;

        JointDrive yzDrive = _joint.angularYZDrive;
        yzDrive.positionSpring = _val;
        _joint.angularYZDrive = yzDrive;
    }

    // Stop the old "Infinite Jumps" problem
    public void SetGroundedState(bool _state)
    {
        isGrounded = _state;
    }

    public void IncrementQuaternionEulerY(ConfigurableJoint _joint, float _incomingFloat)
    {
        totalRotation += _incomingFloat * spinSpeed * Time.deltaTime;

        // Create a quaternion with the accumulated rotation
        Quaternion newRotation = Quaternion.Euler(0f, totalRotation, 0f);

        // Apply the new rotation to the targetRotation of the ConfigurableJoint
        _joint.targetRotation = newRotation;
    }

    public Rigidbody GetHipsRB(){
        return hips;
    }
#endregion
}
