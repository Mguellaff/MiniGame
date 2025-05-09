using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameController : MonoBehaviour, PlayerInputActions.IMinigameActions
{
    private PlayerInputActions inputActions;
    private UnitProduction unit;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Minigame.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.Minigame.Enable();
    }

    private void OnDisable()
    {
        inputActions.Minigame.Disable();
    }

    public void OnHarvest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                unit = hit.collider.GetComponent<UnitProduction>();
                unit?.Harvest();
            }
        }
    }

}
