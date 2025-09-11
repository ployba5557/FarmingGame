using UnityEngine;
using UnityEngine.InputSystem;

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
        basket,
        chopping,
        mining,
        fishing
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
        UIController.instance.SwitchTool((int)currentTool);
        UIController.instance.SwitchSeed(seedCropType);
    }

    void Update()
    {
        if (UIController.instance != null)
        {
            if (UIController.instance.theIC != null && UIController.instance.theIC.gameObject.activeSelf)
            {
                theRB.linearVelocity = Vector2.zero;
                return;
            }

            if (UIController.instance.theShop != null && UIController.instance.theShop.gameObject.activeSelf)
            {
                theRB.linearVelocity = Vector2.zero;
                return;
            }
        }

        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            theRB.linearVelocity = Vector2.zero;
        }
        else
        {
            moveDirection = moveInput.action.ReadValue<Vector2>().normalized;
            theRB.linearVelocity = moveDirection * moveSpeed;
        }

        HandleToolSwitching();

        anim.SetFloat("horizontal", moveDirection.x); 
        anim.SetFloat("vertical", moveDirection.y);
        anim.SetBool("isMoving", theRB.linearVelocity.magnitude > 0.1f);

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
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }
    }

    void HandleToolSwitching()
    {
        bool hasSwitchedTool = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            if ((int)currentTool >= 6)
            {
                currentTool = ToolType.plough;
            }
            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringCan;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            currentTool = ToolType.chopping;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            currentTool = ToolType.mining;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            currentTool = ToolType.fishing;
            hasSwitchedTool = true;
        }

        if (hasSwitchedTool)
        {
            UIController.instance.SwitchTool((int)currentTool);
        }
    }

    void UseTool()
    {
        GrowBlock block = GridController.instance.GetBlock(toolIndicator.position.x - .5f, toolIndicator.position.y - .5f);
        toolWaitCounter = toolWaitTime;

        if (block != null)
        {
            switch (currentTool)
            {
                case ToolType.plough:
                    if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                    EnergyController.instance.UseEnergy(energyUse_PerAction);
                    block.PloughSoil();
                    anim.SetTrigger("usePlough");
                    break;
                case ToolType.wateringCan:
                    if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                    EnergyController.instance.UseEnergy(energyUse_PerAction);
                    block.WaterSoil();
                    anim.SetTrigger("useWateringCan");
                    break;
                case ToolType.seeds:
                    if (CropController.instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {
                        block.PlantCrop(seedCropType);
                    }
                    break;
                case ToolType.basket:
                    block.HarvestCrop();
                    break;

                case ToolType.chopping:
                    if (!EnergyController.instance.HasEnoughEnergy(energyUse_PerAction)) return;
                    EnergyController.instance.UseEnergy(energyUse_PerAction);
                    Collider2D hit = Physics2D.OverlapPoint(toolIndicator.position);

                    if (hit != null && hit.CompareTag("Tree"))
                    {
                        ChoppableTree tree = hit.GetComponent<ChoppableTree>();
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
                    Collider2D rock = Physics2D.OverlapPoint(toolIndicator.position);

                    if (rock != null && rock.CompareTag("Stone"))
                    {
                        MineableRock stone = rock.GetComponent<MineableRock>();
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
                    Collider2D fishSpot = Physics2D.OverlapPoint(toolIndicator.position);

                    if (fishSpot != null && fishSpot.CompareTag("Fish"))
                    {
                        FishingSpotObject fishable = fishSpot.GetComponent<FishingSpotObject>();
                        if (fishable != null)
                        {
                            fishable.Fish();
                            anim.SetTrigger("useFishing"); // ใช้ท่าทางเดียวกับตัดไม้/ขุดหิน
                        }
                    }
                    break;



            }
        }
    }

    void UpdateToolIndicator()
    {
        toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0f);

        if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
        {
            Vector2 direction = toolIndicator.position - transform.position;
            direction = direction.normalized * toolRange;
            toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
        }

        toolIndicator.position = new Vector3(
            Mathf.FloorToInt(toolIndicator.position.x) + .5f,
            Mathf.FloorToInt(toolIndicator.position.y) + .5f, 0f);
    }

    public void SwitchSeed(CropController.CropType newSeed)
    {
        seedCropType = newSeed;
    }


}
