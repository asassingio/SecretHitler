using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoleDragOld : MonoBehaviour
{
    [Header("Role prefab")] 
    [SerializeField] private GameObject secretRolePrefab;
    [SerializeField] private float xPositionIncrement = 250;
    [SerializeField] private float xDragIncrement = 1;
    
    [SerializeField] private float xSizeMax = 420f;
    [SerializeField] private float ySizeMax = 560f;

    [SerializeField] private float smallSize = 0.5f;
    [SerializeField] private float smallYPosition = -70f;

    public SecretRoleEnum SelectedRole { get; private set; }
    private readonly List<GameObject> _roleObjects = new List<GameObject>();

    /*private void Start()
    {
        if (secretRolePrefab == null)
        {
            throw new Exception("Role prefab is not assigned.");
        }
        
        SetRoleIcons();
        
        // trigger component
        EventTrigger evTrig = gameObject.AddComponent<EventTrigger>();

        // Drag event
        EventTrigger.Entry dragEvent = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Drag
        };
        dragEvent.callback.AddListener(MoveRoles);
        evTrig.triggers.Add(dragEvent);

        // EndDrag event
        EventTrigger.Entry endDragEvent = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.EndDrag
        };
        endDragEvent.callback.AddListener(EndMoveRoles); // callback method
        evTrig.triggers.Add(endDragEvent); // callback method parameters
    }

    private void SetRoleIcons()
    {
        // clear the list and destroy every prefab
        if (_roleObjects.Count > 0)
        {
            foreach (GameObject roleObject in _roleObjects)
            {
                Destroy(roleObject);
            }
            _roleObjects.Clear();
        }
        
        // set the size of the parent transform
        RectTransform parentRectTransform = GetComponent<RectTransform>();
        parentRectTransform.sizeDelta = new Vector2(xSizeMax, ySizeMax);
        
        // set the image for every prefab and add it on the list
        foreach (SecretRoleEnum secretRoleEnum in Enum.GetValues(typeof(SecretRoleEnum)))
        {
            Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
            Quaternion spawnRotation = Quaternion.identity;
            GameObject prefabInstance = Instantiate(secretRolePrefab, spawnPosition, spawnRotation, this.transform);
            
            SecretRoleForScrollablePrefab secretRole = prefabInstance.GetComponent<SecretRoleForScrollablePrefab>();
            
            if (secretRole != null)
            {
                secretRole.SetPrefab(secretRoleEnum);
            }
            else
            {
                Debug.LogError($"{secretRole.GetType()} script not found on the spawned prefab {secretRoleEnum}!");
            }
        
            // Add prefab to the list
            _roleObjects.Add(prefabInstance);

            ResetPosition();
            ResizeAllObjects();
        } 
    }
    
    private void MoveRoles(BaseEventData enventData)
    {
        if (enventData is PointerEventData pointerData)
        {
            // Get the drag movement
            Vector2 dragMovement = pointerData.delta;
        
            // Move all the role objects by dragMovement*xDragIncrement
            foreach (GameObject roleObject in _roleObjects)
            {
                Vector3 rolePosition = roleObject.transform.localPosition;
                rolePosition.x += dragMovement.x * xDragIncrement;
                roleObject.transform.localPosition = rolePosition;
                
                
                if (roleObject.transform.localPosition.x <= -(xPositionIncrement*2+10))
                {
                    Vector3 newPosition = new Vector3((_roleObjects.Count-1)*xPositionIncrement, 0f, 0f);
                    roleObject.transform.localPosition = newPosition;
                }
                else if (roleObject.transform.localPosition.x >= xPositionIncrement*2+10)
                {
                    Vector3 newPosition = new Vector3(-(_roleObjects.Count-1)*xPositionIncrement, 0f, 0f);
                    roleObject.transform.localPosition = newPosition;
                }
            }

            
            // create infinite loop
            if (_roleObjects.First().transform.localPosition.x <= -(xPositionIncrement*2+10))
            {
                Vector3 newPosition = _roleObjects.Last().transform.localPosition;
                newPosition.x = _roleObjects.Last().transform.localPosition.x + xPositionIncrement;
                _roleObjects.First().transform.localPosition = newPosition;
            }
            else if (_roleObjects.Last().transform.localPosition.x >= xPositionIncrement*2+10)
            {
                Vector3 newPosition = _roleObjects.First().transform.localPosition;
                newPosition.x = _roleObjects.First().transform.localPosition.x + xPositionIncrement;
                _roleObjects.Last().transform.localPosition = newPosition;
            }

            _roleObjects.Clear();
            _roleObjects.AddRange(_roleObjects.OrderBy(roleObject => roleObject.transform.localPosition.x).ToList());

            ResizeAllObjects();
        }
    }

    /// <summary>
    /// This method is called when the dragging of role objects ends.
    /// It finds the closest prefab to 0 X localPosition and reset the localPosition for all of them with the increment
    /// </summary>
    private void EndMoveRoles(BaseEventData enventData)
    {
        GameObject closestPrefab = null;
        
        float closestDistance = Mathf.Infinity;
        foreach (GameObject roleObject in _roleObjects)
        {
            float distance = Mathf.Abs(roleObject.transform.localPosition.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPrefab = roleObject;
            }
        }

        if (closestPrefab == null)
        {
            Debug.LogError("something went wrong");
            return;
        }

        SelectedRole = closestPrefab.GetComponent<SecretRoleForScrollablePrefab>().SecretRoleEnum;

        ResetPosition();
        ResizeAllObjects();
    }

    /// <summary>
    /// Reset all objects localPosition in the <see cref="RoleDrag"/> class.
    /// the increment is xPositionIncrement.
    /// The first element will have a negative localPosition
    /// </summary>
    private void ResetPosition()
    {
        float xPosition = -xPositionIncrement;
        foreach (GameObject roleObject in _roleObjects)
        {
            Vector3 newPosition = new Vector3(0f, 0f, 0f);
            newPosition.x = xPosition;
            roleObject.transform.localPosition = newPosition;

            // Increment the X localPosition for the next prefab
            xPosition += xPositionIncrement;
        }
    }

    /// <summary>
    /// Resizes all objects in the <see cref="RoleDrag"/> class.
    /// form 1 when the x localPosition is 0, to 0.5 when the x localPosition is >= xPositionIncrement 
    /// </summary>
    private void ResizeAllObjects()
    {
        return;
        foreach (GameObject roleObject in _roleObjects)
        {
            float interpolateValue = Mathf.Clamp01(transform.localPosition.x / xPositionIncrement);
            float newSize = Mathf.Lerp(1f, 0.5f, interpolateValue);
            roleObject.transform.localScale = new Vector3(newSize, newSize, newSize);
        }
    }*/
}

