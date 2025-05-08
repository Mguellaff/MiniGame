using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameController : MonoBehaviour, PlayerInputActions.IMinigameActions
{
    private PlayerInputActions inputActions;

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
                var unit = hit.collider.GetComponent<UnitProduction>();
                if (unit != null)
                {
                    Debug.Log("Harvesting unit: " + unit.name);
                }
            }
        }
    }

}
