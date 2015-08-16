using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public enum PointerEventType
{
    PointerEnter,
    PointerExit,
    PointerDown,
    PointerUp,
    PointerClick,
    Scroll,
    BeginDrag,
    Drag,
    EndDrag,
    Select,
    Deselect
}

[AddComponentMenu("Event/Pointer Event Trigger")]
public class PointerTrigger :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IScrollHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Serializable]
    public class PointerCallback : UnityEvent<PointerEventData>
    {
    }

    [Serializable]
    public class Entry
    {
        public PointerEventType eventID = PointerEventType.PointerClick; // by default
        public PointerCallback callback = new PointerCallback();
    }

    public List<Entry> delegates;

    protected PointerTrigger()
    {
    }

    private void Execute<T>(PointerEventType id, T eventData) where T : PointerEventData
    {
        if (delegates != null)
        {
            for (int i = 0, imax = delegates.Count; i < imax; ++i)
            {
                Entry ent = delegates[i];
                if (ent.eventID == id && ent.callback != null)
                    ent.callback.Invoke(eventData);
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Execute(PointerEventType.PointerEnter, eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Execute(PointerEventType.PointerExit, eventData);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Execute(PointerEventType.PointerDown, eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Execute(PointerEventType.PointerUp, eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Execute(PointerEventType.PointerClick, eventData);
    }

    public virtual void OnScroll(PointerEventData eventData)
    {
        Execute(PointerEventType.Scroll, eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Execute(PointerEventType.BeginDrag, eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Execute(PointerEventType.Drag, eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Execute(PointerEventType.EndDrag, eventData);
    }

    public void Subscribe(PointerEventType type, UnityAction<PointerEventData> handler)
    {
        var trigger = delegates.Find(x => x.eventID == type);
        if (trigger == null)
        {
            var entry = new Entry();
            entry.eventID = type;
            entry.callback.AddListener(handler);
            delegates.Add(entry);
        }
        else trigger.callback.AddListener(handler);
    }
    //public void Subscribe(PointerEventType type, PointerCallback handler)
    //{
    //    var trigger = delegates.Find(x => x.eventID == type);
    //    if (trigger == null)
    //    {
    //        var entry = new Entry();
    //        entry.eventID = type;
    //        entry.callback.AddListener(handler.Invoke);
    //        delegates.Add(entry);
    //    }
    //    else trigger.callback.AddListener(handler.Invoke);
    //}
}