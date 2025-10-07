using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Rigidbody2D theRB;
    public float moveSpeed;

    public InputActionReference moveInput, actionInput;

    public Animator anim;
    public float energyUse_PerAction = 1f;

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket,      // Index 3: เก็บพืชผล, เห็ด, และเนื้อสัตว์
        chopping,
        mining,
        fishing,
        gathering    // Index 7 (เครื่องมือสุดท้าย)
    }
    public ToolType currentTool;

    public float toolWaitTime = 0f;
    private float toolWaitCounter;

    public Transform toolIndicator;
    public float toolRange = 3f;

    public CropController.CropType seedCropType;

    private Vector2 moveDirection;

    void Start()
    {
        if (UIController.instance != null)
        {
            UIController.instance.SwitchTool((int)currentTool);
            UIController.instance.SwitchSeed(seedCropType);
        }
    }

    void Update()
    {
        // 1. ตรวจสอบสถานะ UI/หยุดการเคลื่อนที่
        if (UIController.instance != null)
        {
            if ((UIController.instance.theIC != null && UIController.instance.theIC.gameObject.activeSelf) || 
                (UIController.instance.theShop != null && UIController.instance.theShop.gameObject.activeSelf))
            {
                theRB.linearVelocity = Vector2.zero;
                return;
            }
        }

        // 2. จัดการ Tool Cooldown
        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            theRB.linearVelocity = Vector2.zero;
        }
        else
        {
            // 3. จัดการการเคลื่อนที่ของผู้เล่น
            moveDirection = moveInput.action.ReadValue<Vector2>().normalized;
            theRB.linearVelocity = moveDirection * moveSpeed;
        }

        // 4. จัดการการเปลี่ยนเครื่องมือ
        HandleToolSwitching();

        // 5. จัดการ Animation
        anim.SetFloat("horizontal", moveDirection.x); 
        anim.SetFloat("vertical", moveDirection.y);
        anim.SetBool("isMoving", theRB.linearVelocity.magnitude > 0.1f);

        // 6. จัดการการใช้งานเครื่องมือ
        if (GridController.instance != null)
        {
            if (actionInput.action.WasPressedThisFrame())
            {
                UseTool();
            }

            UpdateToolIndicator();
        }
        else
        {
            // ซ่อน Tool Indicator เมื่อไม่มี GridController (เช่น ตอนโหลดฉาก)
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }
    }

    void HandleToolSwitching()
    {
        bool hasSwitchedTool = false;
        // Max Tool Index คือ 7
        int maxToolIndex = 7; 

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            // วนกลับไปที่ plough หากเกิน ToolType สุดท้าย
            if ((int)currentTool > maxToolIndex) 
            {
                currentTool = ToolType.plough;
            }
            hasSwitchedTool = true;
        }

        // ผูกปุ่มตัวเลขกับ ToolType
        if (Keyboard.current.digit1Key.wasPressedThisFrame) { currentTool = ToolType.plough; hasSwitchedTool = true; }
        if (Keyboard.current.digit2Key.wasPressedThisFrame) { currentTool = ToolType.wateringCan; hasSwitchedTool = true; }
        if (Keyboard.current.digit3Key.wasPressedThisFrame) { currentTool = ToolType.seeds; hasSwitchedTool = true; }
        if (Keyboard.current.digit4Key.wasPressedThisFrame) { currentTool = ToolType.basket; hasSwitchedTool = true; }
        if (Keyboard.current.digit5Key.wasPressedThisFrame) { currentTool = ToolType.chopping; hasSwitchedTool = true; }
        if (Keyboard.current.digit6Key.wasPressedThisFrame) { currentTool = ToolType.mining; hasSwitchedTool = true; }
        if (Keyboard.current.digit7Key.wasPressedThisFrame) { currentTool = ToolType.fishing; hasSwitchedTool = true; }
        // 💡 เราใช้ ToolType.gathering เป็น Index 7 ซึ่งปุ่ม 7 ถูกผูกกับ fishing ไปแล้ว 
        // ถ้าต้องการผูกปุ่ม 8 ให้กับ gathering ให้เปลี่ยน maxToolIndex เป็น 8 และเพิ่มบรรทัดนี้:
        if (Keyboard.current.digit8Key.wasPressedThisFrame) { currentTool = ToolType.gathering; hasSwitchedTool = true; }


        if (hasSwitchedTool && UIController.instance != null)
        {
            UIController.instance.SwitchTool((int)currentTool);
        }
    }

    void UseTool()
    {
        // 🎯 ตำแหน่งที่ผู้เล่นเล็งอยู่ (กึ่งกลางของช่องกริด)
        Vector3 indicatorPos = toolIndicator.position;
        GrowBlock block = GridController.instance.GetBlock(indicatorPos.x - .5f, indicatorPos.y - .5f);
        toolWaitCounter = toolWaitTime;

        // สำหรับ OverlapCircle 
        int gatherableLayerMask = 1 << LayerMask.NameToLayer("spot"); 
        float overlapRadius = 0.1f;
        Collider2D hitSpot = Physics2D.OverlapCircle(indicatorPos, overlapRadius, gatherableLayerMask);


        switch (currentTool)
        {
            case ToolType.plough:
                if (block != null)
                {
                    if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                    EnergyController.instance.UseEnergy(energyUse_PerAction);
                    block.PloughSoil();
                    anim.SetTrigger("usePlough");
                }
                break;

            case ToolType.wateringCan:
                if (block != null)
                {
                    if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                    EnergyController.instance.UseEnergy(energyUse_PerAction);
                    block.WaterSoil();
                    anim.SetTrigger("useWateringCan");
                }
                break;

            case ToolType.seeds:
                if (block != null && CropController.instance != null)
                {
                    if (CropController.instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {
                        block.PlantCrop(seedCropType);
                    }
                }
                break;

            case ToolType.basket:
                // 1. ตรวจสอบการเก็บพืชผลบนพื้นดิน (ถ้ามี)
                if (block != null)
                {
                    block.HarvestCrop(); 
                }
                
                if (hitSpot != null)
                {
                    // 2. เก็บเห็ด
                    if (hitSpot.CompareTag("Mushroom"))
                    {
                        GatherableMushroom mushroom = hitSpot.GetComponent<GatherableMushroom>();
                        if (mushroom != null)
                        {
                            if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                            EnergyController.instance.UseEnergy(energyUse_PerAction); 
                            mushroom.Gather(); 
                            anim.SetTrigger("useBasket"); 
                        }
                    }
                    // 🚩 3. เก็บสัตว์ (เนื้อ) - Logic นี้คือส่วนที่เก็บเนื้อสัตว์
                    else if (hitSpot.CompareTag("Animal")) 
                    {
                        GatherableAnimal animal = hitSpot.GetComponent<GatherableAnimal>();
                        if (animal != null)
                        {
                            if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                            EnergyController.instance.UseEnergy(energyUse_PerAction); 
                            animal.Gather(); // เรียกใช้เมธอดเก็บเนื้อ
                            anim.SetTrigger("useBasket"); 
                        }
                    }
                }
                break;


            case ToolType.chopping:
                if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                EnergyController.instance.UseEnergy(energyUse_PerAction);

                if (hitSpot != null && hitSpot.CompareTag("Tree"))
                {
                    ChoppableTree tree = hitSpot.GetComponent<ChoppableTree>();
                    if (tree != null)
                    {
                        tree.Chop();
                        anim.SetTrigger("useChop"); 
                    }
                }
                break;

            case ToolType.mining:
                if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                EnergyController.instance.UseEnergy(energyUse_PerAction);

                if (hitSpot != null && hitSpot.CompareTag("Stone"))
                {
                    MineableRock stone = hitSpot.GetComponent<MineableRock>();
                    if (stone != null)
                    {
                        stone.Mine();
                        anim.SetTrigger("useChop");
                    }
                }
                break;

            case ToolType.fishing:
                if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                EnergyController.instance.UseEnergy(energyUse_PerAction);

                if (hitSpot != null && hitSpot.CompareTag("Fish"))
                {
                    FishingSpotObject fishable = hitSpot.GetComponent<FishingSpotObject>();
                    if (fishable != null)
                    {
                        fishable.Fish();
                        anim.SetTrigger("useFishing");
                    }
                }
                break;

            case ToolType.gathering:
                // ToolType.gathering (Index 7) สงวนไว้
                break;
        } 
    }
    
    void UpdateToolIndicator()
    {
        // 1. แปลงตำแหน่งเมาส์เป็นพิกัดโลก
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        // 2. จำกัดระยะห่างของ Tool Indicator
        if (Vector3.Distance(mouseWorldPos, transform.position) > toolRange)
        {
            Vector2 direction = mouseWorldPos - transform.position;
            direction = direction.normalized * toolRange;
            mouseWorldPos = transform.position + new Vector3(direction.x, direction.y, 0f);
        }

        // 3. จัดตำแหน่งให้ตรงกลางช่องกริด (Grid Snapping)
        toolIndicator.position = new Vector3(
            Mathf.FloorToInt(mouseWorldPos.x) + .5f,
            Mathf.FloorToInt(mouseWorldPos.y) + .5f, 0f);
    }

    public void SwitchSeed(CropController.CropType newSeed)
    {
        seedCropType = newSeed;
    }
}