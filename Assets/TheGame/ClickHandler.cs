using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheGame
{
    public abstract class ClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public abstract void OnPointerClick(PointerEventData eventData);
    }

    public class MapClickHandler : ClickHandler
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}

