using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private float _checkRate = 0.05f;
    private float _lastCheckTime;

    public float maxCheckDistance;
    public LayerMask layerMask;
    public GameObject curInteractGameObject;
    public IInteractable curInteractable;

    public GameObject prompPanel;
    public TextMeshProUGUI prompTextName;
    public TextMeshProUGUI prompTextDescription;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - _lastCheckTime > _checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPrompText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                prompPanel.gameObject.SetActive(false);
            }
        }

    }

    private void SetPrompText()
    {
        prompPanel.gameObject.SetActive(true);
        prompTextName.text = curInteractable.GetInteractName();
        prompTextDescription.text = curInteractable.GetInteractDescription();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            prompPanel.gameObject.SetActive(false);
        }
    }
}
