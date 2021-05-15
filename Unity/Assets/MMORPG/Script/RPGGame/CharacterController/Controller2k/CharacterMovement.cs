using System;
using System.Collections.Generic;
using UnityEngine;
using Controller2k;
[RequireComponent(typeof(AudioSource))]
public class CharacterMovement : Movement
{
    [Header("Components")]
    public Player player;
    public Animator animator;
    //public Health health;
    //public Combat combat;
    public CapsuleCollider controllerCollider;
    public CharacterController2k controller;
    
    [Header("Animation")]
    public float directionDampening = 0.05f;
    public float turnDampening = 0.1f;
    Vector3 lastForward;
    Move lastMove;

    [Header("State")]
    public MoveState state = MoveState.IDLE;
    MoveState lastState = MoveState.IDLE;
    [HideInInspector] public Vector3 moveDir;

    // 支持movement (WASD) and rotations (QE)

    [Header("Moving")]
    [Range(0f, 1f)] public float runStepLength = 0.7f;
    public float runStepInterval = 3;
    public float runCycleLegOffset = 0.2f; 
    public float moveSpeed = 2f;
    public float walkSpeed = 2f;
	public float sprintSpeed = 7.5f; 
	public float speedDampTime = 0.1f;
    private float speed, speedSeeker; 
    float stepCycle;
    float nextStep;
    public bool runWhileBothMouseButtonsPressed = true;

    [Header("Rotation")]
    public float rotationSpeed = 190;

    [Header("Mounted")]
    public float mountedRotationSpeed = 100;

    [Header("Physics")]
    [Tooltip("在地面上施加一个小的默认向下的力，以便粘在地面和圆形的表面上。否则，在圆形表面行走将被视为跌倒，从而阻止玩家跳跃。")]
    public float gravityMultiplier = 2;

    [Header("Swimming")]
    public float swimSpeed = 4;
    public float swimSurfaceOffset = 0.25f;
    Collider waterCollider;
    bool inWater => waterCollider != null; 
    bool underWater; 
    public LayerMask canStandInWaterCheckLayers = Physics.DefaultRaycastLayers; 
    [Header("Swimming [CAREFUL]")]
    [Range(0, 1)] public float underwaterThreshold = 0.7f; 

    [Header("Jumping")]
    public float jumpSpeed = 7;
    [HideInInspector] public float jumpLeg;
    bool jumpKeyPressed;

    [Header("Airborne")]
    public bool airborneSteering = true;
    public float fallMinimumMagnitude = 6; 
    public float fallDamageMinimumMagnitude = 13;
    public float fallDamageMultiplier = 2;
    [HideInInspector] public Vector3 lastFall;


    [Header("Synchronization (Best not to modify)")]
    [Tooltip("在FixedUpdate中应用这些移动之前，至少缓冲这些移动。一个更大的最小缓冲区提供了更多的延迟容忍和额外的延迟成本。")]
    public int minMoveBuffer = 2;
    [Tooltip("为了避免不断增长的队列，在有了这么多的挂起的移动之后，将两个移动合并为一个。")]
    public int combineMovesAfter = 3;
    [Tooltip("在强制重置客户端之前，最多缓冲多次移动。缓冲上百步的移动，总是落后3秒是没有意义的。")]
    public int maxMoveBuffer = 10; // 20ms fixedDelta * 10 = 200 ms. 上百步接近3秒
    [Tooltip("橡皮筋移动:玩家可以自由移动，只要服务器位置匹配即可。如果服务器和客户端断开的距离大于rubberDistance，则会发生强制复位。")]
    public float rubberDistance = 1;
    [Tooltip("最大移动速度的公差百分比。0%太小，因为总有一些延迟和不精确。100%太多了，因为它允许双倍速度的黑客攻击。")]
    [Range(0, 1)] public float validSpeedTolerance = 0.2f;
    

    // 用于float/vector3比较的epsilon（由于在网络上发送时不精确等原因而需要）
    const float epsilon = 0.1f;

    public bool isGroundedWithinTolerance =>
        controller.isGrounded || controller.velocity.y > -fallMinimumMagnitude;
    public Vector3 velocity { get; private set; }
    float lastClientSendTime;

    // public NetworkScene networkScene = null;
    void Start()
    {
        //networkScene = NetworkScene.singleton;
        //networkScene.StartNetwork();

        playerCamera = Camera.main.transform;
    }

    void Update(){
        // 当输入与交易等状态时，不允许移动
        if (player.IsMovementAllowed())
        {
            if (!jumpKeyPressed)
            {
                jumpKeyPressed = Input.GetButtonDown("Jump");
                // jump = true;
            }  
        }

        // 所有player角色动画更新
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        Vector3 dir = transform.InverseTransformDirection(moveDir);

        float turnAngle = AnimationDeltaUnclamped(lastForward, transform.forward);
        lastForward = transform.forward;

        // 将动画参数应用于所有动画，角色使用了多个蒙皮网格的装备道具。
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
            animator.SetFloat("DirX", dir.x, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirY", dir.y, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirZ", dir.z, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            // animator.SetFloat("LastFallY", lastFall.y);
            // animator.SetFloat("Turn", turnAngle, turnDampening, Time.deltaTime); // smooth turn
            // animator.SetBool("SWIMMING", state == MoveState.SWIMMING);

            // animator.SetBool("Grounded", state != MoveState.AIRBORNE);
            // if (controller.isGrounded) animator.SetFloat("JumpLeg", GetJumpLeg());
        }
    }

    void FixedUpdate()
    {
        JumpManagement();
        // 控制本地玩家角色
        if (isLocalPlayer)
        {
            // 根据摄像机和地面获取输入和所需方向
            Vector2 inputDir = player.IsMovementAllowed() ? GetInputDirection() : Vector2.zero;
            Vector3 desiredDir = GetDesiredDirection(inputDir);
            Debug.DrawLine(transform.position, transform.position + desiredDir, Color.cyan);

            // update 状态机
            if      (state == MoveState.IDLE)             state = UpdateIDLE(inputDir, desiredDir);
            else if (state == MoveState.RUNNING)          state = UpdateRUNNING(inputDir, desiredDir);
            else if (state == MoveState.AIRBORNE)         state = UpdateAIRBORNE(inputDir, desiredDir);
            else if (state == MoveState.SWIMMING)         state = UpdateSWIMMING(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED)          state = UpdateMOUNTED(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED_AIRBORNE) state = UpdateMOUNTED_AIRBORNE(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED_SWIMMING) state = UpdateMOUNTED_SWIMMING(inputDir, desiredDir);
            //else if (state == MoveState.DEAD)             state = UpdateDEAD(inputDir, desiredDir);
            else Debug.LogError("Unhandled Movement State: " + state);

            // 调用角色旋转
		    Rotating(inputDir.x, inputDir.y);

            speed = Vector2.ClampMagnitude(inputDir, 1f).magnitude;
            // 鼠标中键盘滚动切换速度
            speedSeeker = 4;
            speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, sprintSpeed);
            moveSpeed = speed *= speedSeeker;

            // 缓存此移动的状态，以便下次检测着陆等
            if (!controller.isGrounded) lastFall = controller.velocity;

            // 移动取决于最新的moveDir更改
            controller.Move(moveDir * Time.fixedDeltaTime); // note: returns CollisionFlags if needed
            velocity = controller.velocity; // for animations and fall damage

            // 移动取决于//计算哪条腿在后面，以便在跳跃动画中留下那条腿在后面
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            jumpLeg = (runCycle < 0.5f ? 1 : -1); 
            // 不管发生什么，都要重置keys
            jumpKeyPressed = false;

            // 向服务器send Move
            CmdFixedMove(new Move(state, transform.position, transform.rotation.eulerAngles.y,speed,jumpLeg));
        }

        // 在所有其他操作完成后设置最后一个状态
        lastState = state;
    }

    void JumpManagement()
	{
		// 起跳
		if (jumpKeyPressed && !animator.GetBool("Jump") && controller.isGrounded)
		{
			animator.SetBool("Jump", true);
		}
		// 跳后着地
		else if (animator.GetBool("Jump") && controller.isGrounded)
		{
            animator.SetBool("Jump", false);
			
		}
	}

    private Transform playerCamera;
    public float turnSmoothing = 0.5f;  
    private Vector3 lastDirection;
    void Rotating(float h, float v)
	{
		Vector3 targetDirection;

		// 相机朝向
		Vector3 forward = playerCamera.TransformDirection(Vector3.forward);

		// forward的单位向量
		forward.y = 0.0f;
		forward = forward.normalized;

		// ws为V,ad为H,right:z正方向，0,-x
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		// 向量*方向键输入变量并将两个向量相加
		targetDirection = forward * v + right * h;

		// Lerp current direction to calculated target direction.
		if(targetDirection != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection);

			Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
			transform.rotation = newRotation;
			lastDirection =  targetDirection;
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if(!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
		{
			Repositioning();
		}
	}
    public void Repositioning()
	{
		if(lastDirection != Vector3.zero)
		{
			lastDirection.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (lastDirection);
			Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
			transform.rotation = newRotation;
		}
	}

    // 调用网络层,传出Move数据
    void CmdFixedMove(Move move)
    {
        // 调用网络层RpcSendMoveToServer方法,向附近所有玩家广播
        // if(networkScene!=null) networkScene.NetworkMove(move);
    }

    Queue<Move> pendingMoves = new Queue<Move>();
    // 被网络层调用,传入Move数据
    public void RpcFixedMove(Move move)
    {        
        // scale character collider to pose if not local player.
        // -> correct collider is needed both on server and on clients
        if (lastState != state)
            AdjustControllerCollider();

        // 缓存此移动的状态，以便下次检测着陆等
        if (!controller.isGrounded) lastFall = controller.velocity;
            
        Vector3 next = move.position - transform.position; // calculate the delta before each move. using .position is 100% accurate and never gets out of sync.

        moveSpeed = speed = move.nSpeed;
        state = move.state;
        transform.rotation = Quaternion.Euler(0, move.yRotation, 0);
        controller.Move(next);
        
        float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
        jumpLeg = (runCycle < 0.5f ? 1 : -1); 

        // 在所有其他操作完成后设置最后一个状态
        lastState = state;
}


    // rotate with QE keys
    void RotateWithKeys()
    {
        float horizontal2 = Input.GetAxis("Horizontal2");
        transform.Rotate(Vector3.up * horizontal2 * rotationSpeed * Time.fixedDeltaTime);
    }

    //调整角色控制器碰撞器的姿势，否则我们朝在爬行姿态（游泳姿态等）的玩家上方开火，仍然击中他。
    void AdjustControllerCollider()
    {
        // ratio depends on state
        float ratio = 1;
        if (state == MoveState.SWIMMING || state == MoveState.DEAD)
            ratio = 0.25f;

        controller.TrySetHeight(controller.defaultHeight * ratio, true, true, false);
    }


    // check if a movement vector is valid (the 'rubber' part)
    // ('move' is a movement vector, NOT a position!)
    bool IsValidMove(Vector3 move)
    {
        // 玩家发送Vector3.Zero,包括空闲，施放、旋转、改变状态等。
        // ->这些应该总是被允许的。否则我们将发送无效的移动消息到控制台。
        if (move.magnitude <= epsilon)
            return true;

        // 我们是否处于允许移动的状态（没有死亡等）？
        return player.IsMovementAllowed();
    }

    bool EventDied()
    {
        //return health.current == 0;
        return false;
    }

    bool EventJumpRequested()
    {
        return isGroundedWithinTolerance &&
               controller.slidingState == SlidingState.NONE &&
               jumpKeyPressed;
    }

    bool EventFalling()
    {
        return !isGroundedWithinTolerance;
    }

    bool EventLanded()
    {
        return controller.isGrounded;
    }

    void OnTriggerEnter(Collider co)
    {
        //print(co.name);
        // touching water? then set water collider
        if (co.tag == "Water")
            waterCollider = co;
    }

    void OnTriggerExit(Collider co)
    {
        if (co.tag == "Water")
            waterCollider = null;
    }
    
    // animations //////////////////////////////////////////////////////////////
    float GetJumpLeg()
    {
        return jumpLeg;
    }

    void ApplyFallDamage()
    {
        //仅测量Y方向。
        float fallMagnitude = Mathf.Abs(lastFall.y);
        if(fallMagnitude >= fallDamageMinimumMagnitude)
        {
            int damage = Mathf.RoundToInt(fallMagnitude * fallDamageMultiplier);
            // health.current -= damage;
            // combat.RpcOnReceivedDamaged(damage, DamageType.Normal);
        }
    }

    static float AnimationDeltaUnclamped(Vector3 lastForward, Vector3 currentForward)
    {
        Quaternion rotationDelta = Quaternion.FromToRotation(lastForward, currentForward);
        float turnAngle = rotationDelta.eulerAngles.y;
        return turnAngle >= 180 ? turnAngle - 360 : turnAngle;
    }

    

    bool EventUnderWater()
    {
        if (inWater) 
        {
            // 从水底到玩家位置的光线投射
            Vector3 origin = new Vector3(transform.position.x,
                                         waterCollider.bounds.max.y,
                                         transform.position.z);
            float distance = controllerCollider.height * underwaterThreshold;
            Debug.DrawLine(origin, origin + Vector3.down * distance, Color.cyan);

            // 如果光线投射没有击中任何东西就在水下
            return !Utils.RaycastWithout(origin, Vector3.down, out RaycastHit hit, distance, gameObject, canStandInWaterCheckLayers);
        }
        return false;
    }

    float ApplyGravity(float moveDirY)
    {
        // 坠落时施加全重力
        if (!controller.isGrounded)
            return moveDirY + Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        return 0;
    }


    MoveState UpdateIDLE(Vector2 inputDir, Vector3 desiredDir)
    {
        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * moveSpeed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * moveSpeed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.AIRBORNE;
        }
        else if (EventUnderWater())
        {
            // rescale capsule
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }
        else if (inputDir != Vector2.zero)
        {
            return MoveState.RUNNING;
        }

        return MoveState.IDLE;
    }

    MoveState UpdateRUNNING(Vector2 inputDir, Vector3 desiredDir)
    {
        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * moveSpeed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * moveSpeed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.AIRBORNE;
        }
        else if (EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }
        // 完全减速后进入 idle (y无所谓)
        else if (moveDir.x == 0 && moveDir.z == 0)
        {
            return MoveState.IDLE;
        }

        ProgressStepCycle(inputDir, moveSpeed);
        return MoveState.RUNNING;
    }

    MoveState UpdateAIRBORNE(Vector2 inputDir, Vector3 desiredDir)
    {
        if (airborneSteering)
        {
            // QE key rotation
            if (player.IsMovementAllowed())
                RotateWithKeys();

            // move
            moveDir.x = desiredDir.x * moveSpeed;
            moveDir.y = ApplyGravity(moveDir.y);
            moveDir.z = desiredDir.z * moveSpeed;
        }
        // 否则，继续向同一方向移动，仅施加重力
        else
        {
            moveDir.y = ApplyGravity(moveDir.y);
        }

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventLanded())
        {
            PlayLandingSound();
            return MoveState.IDLE;
        }
        else if (EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }

        return MoveState.AIRBORNE;
    }

    MoveState UpdateSWIMMING(Vector2 inputDir, Vector3 desiredDir)
    {
        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        // not under water anymore?
        else if (!EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 1f, true, true, false))
            {
                return MoveState.IDLE;
            }
        }

        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * swimSpeed;
        moveDir.z = desiredDir.z * swimSpeed;

        // gravitate toward surface
        if (waterCollider != null)
        {
            float surface = waterCollider.bounds.max.y;
            float surfaceDirection = surface - controller.bounds.min.y - swimSurfaceOffset;
            moveDir.y = surfaceDirection * swimSpeed;
        }
        else moveDir.y = 0;

        return MoveState.SWIMMING;
    }

    MoveState UpdateMOUNTED(Vector2 inputDir, Vector3 desiredDir)
    {
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // 还没有加入坐骑判断，如果有坐骑速度要切换
        float speed = moveSpeed;

        // move
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.MOUNTED_AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.MOUNTED_AIRBORNE;
        }
        else if (EventUnderWater())
        {
            return MoveState.MOUNTED_SWIMMING;
        }

        return MoveState.MOUNTED;
    }

    MoveState UpdateMOUNTED_AIRBORNE(Vector2 inputDir, Vector3 desiredDir)
    {
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // 还没有加入坐骑判断，如果有坐骑速度要切换
        float speed = moveSpeed;

        // move
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventLanded())
        {
            PlayLandingSound();
            return MoveState.MOUNTED;
        }
        else if (EventUnderWater())
        {
            return MoveState.MOUNTED_SWIMMING;
        }

        return MoveState.MOUNTED_AIRBORNE;
    }

    MoveState UpdateMOUNTED_SWIMMING(Vector2 inputDir, Vector3 desiredDir)
    {
        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        // not under water anymore?
        else if (!EventUnderWater())
        {
            return MoveState.MOUNTED;
        }
        
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // move with acceleration (feels better)
        moveDir.x = desiredDir.x * swimSpeed;
        moveDir.z = desiredDir.z * swimSpeed;

        // gravitate toward surface
        if (waterCollider != null)
        {
            float surface = waterCollider.bounds.max.y;
            float surfaceDirection = surface - controller.bounds.min.y - swimSurfaceOffset;
            moveDir.y = surfaceDirection * swimSpeed;
        }
        else moveDir.y = 0;

        return MoveState.MOUNTED_SWIMMING;
    }

    bool WasValidSpeed(Vector3 moveVelocity, MoveState previousState, MoveState nextState, bool combining)
    {
        float speed = new Vector2(moveVelocity.x, moveVelocity.z).magnitude;

        float maxSpeed = Mathf.Max(GetMaximumSpeedForState(previousState),
                                   GetMaximumSpeedForState(nextState));

        float maxSpeedWithTolerance = maxSpeed * (1 + validSpeedTolerance);

        // we allow twice the speed when applying a combined move
        if (combining)
            maxSpeedWithTolerance *= 2;

        if (speed <= maxSpeedWithTolerance)
        {
            return true;
        }
        else Debug.Log(name + " move rejected because too fast: combining=" + combining + " xz speed=" + speed + " / " + maxSpeedWithTolerance + " state=" + previousState + "=>" + nextState);
        return false;
    }

    float GetMaximumSpeedForState(MoveState moveState)
    {
        switch (moveState)
        {
            // idle, running, mounted use runSpeed which is set by Entity
            case MoveState.IDLE:
            case MoveState.RUNNING:
            case MoveState.MOUNTED:
                return moveSpeed;
            // swimming uses swimSpeed
            case MoveState.SWIMMING:
            case MoveState.MOUNTED_SWIMMING:
                return swimSpeed;
            // airborne accelerates with gravity.
            // maybe check xz and y speed separately.
            case MoveState.AIRBORNE:
            case MoveState.MOUNTED_AIRBORNE:
                return float.MaxValue;
            case MoveState.DEAD:
                return 0;
            default:
                Debug.LogWarning("Don't know how to calculate max speed for state: " + moveState);
                return 0;
        }
    }

    void PlayLandingSound()
    {
        // feetAudio.clip = landSound;
        // feetAudio.Play();
        // nextStep = stepCycle + .5f;
    }

    void PlayJumpSound()
    {
        // feetAudio.clip = jumpSound;
        // feetAudio.Play();
    }

    void ProgressStepCycle(Vector3 inputDir, float speed)
    {
        if (controller.velocity.sqrMagnitude > 0 && (inputDir.x != 0 || inputDir.y != 0))
        {
            stepCycle += (controller.velocity.magnitude + (speed * runStepLength)) *  Time.fixedDeltaTime;
        }

        if (stepCycle > nextStep)
        {
            nextStep = stepCycle + runStepInterval;
            PlayFootStepAudio();
        }
    }

    void PlayFootStepAudio()
    {
        if (!controller.isGrounded) return;

        // do we have any footstep sounds?
        //...
    }


    Vector3 GetDesiredDirection(Vector2 inputDir)
    {
        if(inputDir.y>0) return transform.forward * inputDir.y;
        else if(inputDir.y<0) return -transform.forward * inputDir.y;
        else if(inputDir.x>0) return transform.forward * inputDir.x;
        else if(inputDir.x<0) return -transform.forward * inputDir.x;
        else return transform.forward * inputDir.y + transform.right * inputDir.x;
        //return transform.forward * inputDir.y + transform.right * inputDir.x;
    }

    Vector2 GetInputDirection()
    {
        float horizontal = 0;
        float vertical = 0;

        // keyboard input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        if (input.magnitude > 1)
        {
            input = input.normalized;
        }
        return input;
    }

    public override Vector3 GetVelocity()
    {
        return velocity;
    }

    public override bool IsMoving()
    {
        return velocity != Vector3.zero;
    }

    public override void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public override void Reset()
    {
        // we have no navigation, so we don't need to reset any paths
    }

    public override void LookAtY(Vector3 position)
    {
        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
    }

    public override bool CanNavigate()
    {
        return false;
    }

    public override void Navigate(Vector3 destination, float stoppingDistance)
    {
        // character controller movement doesn't allow navigation (yet)
    }

    public override bool IsValidSpawnPoint(Vector3 position)
    {
        return true;
    }

    public override Vector3 NearestValidDestination(Vector3 destination)
    {
        // character controller movement doesn't allow navigation (yet)
        return destination;
    }

    public override bool DoCombatLookAt()
    {
        // player should use keys/mouse to look at. don't overwrite it.
        return false;
    }

    public override void Warp(Vector3 destination)
    {
        // set new position
        transform.position = destination;
    }
}
