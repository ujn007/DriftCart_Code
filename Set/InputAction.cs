using System;
using UnityEngine;

public class InputAction : MonoBehaviour
{
    public Action<Vector3, bool> OnMoveClickEvent;
    public Action OnBoostEvent;
    public Action<bool> OnPauseEvent;

    [SerializeField] LayerMask whatIsCheckOBJ;

    private void Update()
    {
        OnMouseClickEvent();
        OnBoost();
        OnPause();
    }

    private void OnMouseClickEvent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool checkRay = Physics.Raycast(ray, out RaycastHit hit, 100, whatIsCheckOBJ);

        if (Input.GetMouseButton(0) && checkRay)
        {
            OnMoveClickEvent?.Invoke(hit.point, true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMoveClickEvent?.Invoke(hit.point, false);
        }
    }

    private void OnBoost()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnBoostEvent?.Invoke();
        }
    }

    private void OnPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager gm = GameManager.Instance;
            gm.pauseToggle = !gm.pauseToggle; 
            OnPauseEvent?.Invoke(gm.pauseToggle);
        }
    }
}
