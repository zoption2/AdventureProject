using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Map
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private Spot[] _spots;
    }

    public class Spot : MonoBehaviour
    {
        [SerializeField] private Spot[] _connectedSpots;
        [SerializeField] private SectorType _sectorType;
        private bool _isEmpty;

        public bool IsEmpty => _isEmpty;
    }

    public enum SectorType
    {
        Blue,
        Green,
        Yellow,
        Red,
        Violet,
        Brown
    }

    public enum SpotType
    {

    }
}

