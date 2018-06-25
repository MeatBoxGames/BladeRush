using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Default : MonoBehaviour
{
    public PlayerCharacter controllingCharacter;
    GameObject swordProjectile;

    public float movespeed = 7.0f;
    public float gravscale = 50.0f;
    public Camera camera;
    public Rigidbody rigidbody;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;
    float minimumX = -360.0f;
    float maximumX = 360.0f;
    float minimumY = -90.0f;
    float maximumY = 90.0f;
    float rotationX = 0.0f;
    float rotationY = 0.0f;
    Quaternion originalRotation;

    float refireTimer;
    float maxRefire = 0.25f;

    public float midairPause = 0.25f;
    public float midairAttackRefresh = 0.5f;
    float currAirPause;
    bool bAirPause;
    int triggerCache;
    int attackBoolCache;

    float attackTimer;
    float queueTimer;
    public float maxAttackTimer = 0.5f;
    public float attackQueueTimer = 2.0f;
    public float attackRange = 3.0f;
    public int attackDamage = 35;
    public float attackStunDuration = 1.0f;
    int airPauseLimit;
    bool bAttackQueued;
    bool bReadyToQueue;

    public float currentStamina;
    public float maxStamina = 100.0f;
    public float swordTeleportCost = 25.0f;
    public float staminaRegenDelay = 1.0f;
    public float staminaRegen = 35.0f;
    float staminaTimer;

    Animator swordAnimController;
    bool bSwordVisible = true;
    Renderer swordMesh;

    GameMode game;

    // Use this for initialization
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        originalRotation = transform.localRotation;

        Physics.gravity = new Vector3(0, -1.0f * gravscale, 0);

        controllingCharacter = (PlayerCharacter)GetComponent<Character>();

        setCurrentStamina(maxStamina);
    }

    public void setCurrentStamina(float newstam)
    {
        Debug.Log(newstam);
        currentStamina = Mathf.Clamp(newstam, 0.0f, maxStamina);
        updateStaminaHUD();
    }

    public void setStaminaTimer()
    {
        staminaTimer = staminaRegenDelay;
    }

    void ThrowSword()
    {

        airPauseLimit = 0;
        controllingCharacter.ThrowSword(transform.position, camera.transform.rotation);
    }

    public void enableAirPause()
    {
        bAirPause = true;
        currAirPause = midairPause;
        Physics.gravity = new Vector3(0, 0, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    void disableAirPause()
    {
        currAirPause = 0.0f;
        bAirPause = false;
        Physics.gravity = new Vector3(0, -1.0f * gravscale, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    void updateAirMove()
    {
        if (!bAirPause)
            return;
        else if (currAirPause > 0)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            currAirPause -= Time.deltaTime;
        }
        else if (currAirPause < 0)
        {
            disableAirPause();
        }
    }

    void checkForSword()
    {
        if (swordMesh == null)
        {
            Renderer[] mesh;
            mesh = GetComponentsInChildren<Renderer>();

            foreach (Renderer ms in mesh)
            {
                if (ms.tag == "Entity")
                    swordMesh = ms;
            }
        }

        if (!controllingCharacter.bHasSword && bSwordVisible)
        {
            swordMesh.enabled = false;
            bSwordVisible = false;
        }
        else if (controllingCharacter.bHasSword && !bSwordVisible)
        {
            swordMesh.enabled = true;
            bSwordVisible = true;
        }
    }

    void updateStaminaHUD()
    {
        var objects = GameObject.FindGameObjectsWithTag("Stamina")[0];

        if (objects == null)
            return;

        UnityEngine.UI.Image stamBar = objects.GetComponent<UnityEngine.UI.Image>();
        stamBar.fillAmount = currentStamina / maxStamina;
    }

    void updateStamina()
    {
        if (staminaTimer <= 0)
        {
            float stam = Mathf.Clamp(currentStamina + (staminaRegen * Time.deltaTime), 0.0f, maxStamina);
            setCurrentStamina(stam);
        }
    }

    void Update()
    {
        if (game == null)
            game = FindObjectOfType<GameMode>();

        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);

        MovePlayer();
        updateAirMove();
        checkForSword();
        updateStamina();

        if (refireTimer > 0) refireTimer -= Time.deltaTime;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (queueTimer > 0) queueTimer -= Time.deltaTime;
        if (staminaTimer > 0) staminaTimer -= Time.deltaTime;

        if (Input.GetAxis("Fire2") != 0 && refireTimer <= 0)
        {
            ThrowSword();
            refireTimer = maxRefire;
        }

        if ((Input.GetAxis("Fire1") != 0 || bAttackQueued) && attackTimer <= 0)
            attack();
        else if (Input.GetAxis("Fire1") != 0 && bReadyToQueue)
            bAttackQueued = true;
        else if (queueTimer <= 0 && !bAttackQueued)
            resetAttack();
        else if (Input.GetAxis("Fire1") == 0 && attackTimer <= 0)
            bReadyToQueue = true;

        if (Input.GetAxis("Fire3") != 0)
        {

        }
    }

    void dealDamage()
    {
        var objects = GameObject.FindGameObjectsWithTag("Character");
        var objectCount = objects.Length;

        foreach (var obj in objects)
        {
            if (Vector3.Dot(transform.forward, obj.transform.position - transform.position) < 0.8f)
                continue;

            if (Vector3.Distance(obj.transform.position, transform.position) > attackRange)
                continue;

            Enemy hitEnemy = obj.GetComponent<Enemy>();

            if (hitEnemy != null)
                hitEnemy.takeDamage(attackDamage, attackStunDuration);
        }
    }

    void linkAnimations()
    {
        swordAnimController = GetComponentInChildren<Animator>();
        triggerCache = Animator.StringToHash("EndAttack");
        attackBoolCache = Animator.StringToHash("DoAttack");
    }

    void attack()
    {
        if (swordAnimController == null)
            linkAnimations();

        if (!controllingCharacter.bHasSword)
            return;

        swordAnimController.SetTrigger(attackBoolCache);
        swordAnimController.ResetTrigger(triggerCache);
        attackTimer = maxAttackTimer;
        queueTimer = attackQueueTimer;
        bAttackQueued = false;
        bReadyToQueue = false;
        dealDamage();

        if (airPauseLimit < 3)
        {
            // Bit shift the index of the layer to get a bit mask
            int layermask1 = 1 << 9;
            int layermask2 = 1 << 10;
            int layermask3 = 1 << 11;

            int finalmask = layermask1 | layermask2 | layermask3;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            finalmask = ~finalmask;

            RaycastHit hit;

            if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1.0f), out hit, 2.0f, finalmask))
            {
                enableAirPause();
                airPauseLimit++;
            }
        }
    }

    void resetAttack()
    {
        if (swordAnimController == null)
            linkAnimations();

        swordAnimController.SetTrigger(triggerCache);
        swordAnimController.ResetTrigger(attackBoolCache);
        bAttackQueued = false;
        bReadyToQueue = false;
    }

    void MovePlayer()
    {
        if (!Application.isFocused) return;

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation = originalRotation * xQuaternion;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
        camera.transform.localRotation = originalRotation * yQuaternion;

        Vector3 targetForward = transform.localRotation * Vector3.forward;
        Vector3 targetUp = transform.localRotation * Vector3.up;

        if (bAirPause) return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 9.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 9.0f;

        Vector3 move = transform.forward * z;
        move += transform.right * x;

        move.Normalize();
        move *= movespeed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + move);

        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
    }

    public static float ClampAngle (float angle, float min, float max)
    {
         if (angle < -360.0f)
             angle += 360.0f;
         if (angle > 360.0f)
             angle -= 360.0f;
         return Mathf.Clamp (angle, min, max);
    }
}
