using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/SimpleJoyStick", 51)]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class SimpleJoyStick : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private RectTransform m_Stick;
    [SerializeField]
    private RectTransform m_Background;
    [SerializeField]
    private bool    m_IsLockPostion = false;

    private bool    m_Touched = false;
    private Vector3 m_OriginPos;
    private Vector2 m_Direction;
    private float   m_fRadius;
    private int     m_iPointerID = -100;

    public float xAxis { get { return m_Direction.x; } }
    public float yAxis { get { return m_Direction.y; } }
    public Vector2 direction { get { return m_Direction; } }

    public bool isMoved { get { return m_Touched && direction != Vector2.zero; } }
    public bool IsLockPosition { get { return m_IsLockPostion; } set { this.m_IsLockPostion = value;} }



    protected override void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        m_fRadius = m_Background.rect.width / 2 - m_Stick.rect.width / 2;
        m_Stick.localPosition = Vector3.zero;
        m_OriginPos = m_Background.position;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (m_Touched)
            return;
        m_Touched = true;
        m_iPointerID = eventData.pointerId;
        if (!m_IsLockPostion)
        {
            Vector3 newPosition = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            // Ignore Z Component
            newPosition.z = m_Background.position.z;
            m_Background.position = newPosition;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if(m_iPointerID == eventData.pointerId)
            ResetUI();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (m_iPointerID == eventData.pointerId)
            UpdateEventData(eventData);
    }

#if !UNITY_EDITOR
    public virtual void OnApplicationFocus(bool bFocus)
    {
        if(!bFocus)
        {
            ResetUI();
        }
    }
#endif

    private void ResetUI()
    {
        m_Touched = false;
        m_iPointerID = -100;
        m_Background.position = m_OriginPos;
        m_Direction = Vector2.zero;
        m_Stick.position = m_OriginPos;
    }

    private void UpdateEventData(PointerEventData eventData)
    {
        var UICamera = eventData.pressEventCamera;
        Vector2 backGroundScreenPos = UICamera.WorldToScreenPoint(m_Background.position);

        m_Direction = eventData.position - backGroundScreenPos;
        m_Direction = m_Direction.normalized;

        float Distance = Vector2.Distance(eventData.position,backGroundScreenPos);
        if (Distance > m_fRadius)
        {
            m_Stick.anchoredPosition = new Vector3(xAxis * m_fRadius, yAxis * m_fRadius, 0);
        }
        else
        {
            Vector2 tarPos;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Background,eventData.position,UICamera,out tarPos))
            {
                m_Stick.anchoredPosition = tarPos;
            }
        }
    }
}
