using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    #region Variables

    private Camera _mainCamera;

    #endregion

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;


        if (rayHit.collider.gameObject.CompareTag("Tower"))
        {
            Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);

            TowerParent tower = rayHit.collider.gameObject.GetComponent<TowerParent>();
            tower.UpgradeTower();
        }
    }
}
