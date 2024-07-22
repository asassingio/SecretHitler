using System;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleDrag : MonoBehaviour
{
    [Header("Role prefab")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPortTransform;
    [SerializeField] private RectTransform contentPanelTransform;
    [SerializeField] private HorizontalLayoutGroup hlg;
    [SerializeField] private RectTransform[] itemList;
    
    private Vector2 _oldVelocity;
    private bool _isUpdated;
    
    public SecretRoleEnum SelectedRole { get; private set; }
    // private readonly List<GameObject> _roleObjects = new List<GameObject>();

    private void Start()
    {
        float itemsToAdd = MathF.Ceiling(viewPortTransform.rect.width / (itemList[0].rect.width + hlg.spacing));

        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform rt = Instantiate(itemList[i % itemList.Length], contentPanelTransform);
            rt.SetAsLastSibling();
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = itemList.Length - i - 1;
            while (num < 0)
            {
                num += itemList.Length;
            }

            RectTransform rt = Instantiate(itemList[num], contentPanelTransform);
            rt.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3(
            0 - (itemList[0].rect.width + hlg.spacing) * itemsToAdd,
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);
    }

    void Update()

    {   
        if(_isUpdated){
            _isUpdated = false;
            scrollRect.velocity = _oldVelocity;
        }

        if(contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            _oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);
            _isUpdated = true;
        }
        if(contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            _oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);
            _isUpdated = true;
        }
    }

    public void p()
    {
        Debug.Log("Drag Started");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Started");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OK");
    }
}

