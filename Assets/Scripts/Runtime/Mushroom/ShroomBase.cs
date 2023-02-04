using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamShrimp.GGJ23
{
    public abstract class ShroomBase : MonoBehaviour
    {

        private long _shroomId;

        private Vector2Int _shroomPosition;

        [SerializeField] private ShroomType _shroomType;
        
        private ShroomBase _parent;

        private List<ShroomBase> _children;
        
        private LineRenderer _connector;
        
        public ShroomType ShroomType
        {
            get => _shroomType;
            set => _shroomType = value;
        }
        
        public Vector2Int ShroomPosition
        {
            get => _shroomPosition;
            set => _shroomPosition = value;
        }

        public ShroomBase Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public List<ShroomBase> Children
        {
            get => _children;
            set => _children = value;
        }

        public Vector2Int ParentPosition => Parent.ShroomPosition;

        public long ShroomId => _shroomId;

        public LineRenderer Connector
        {
            get => _connector;
            set => _connector = value;
        }

        public bool IsOfType(ShroomType shroomType) => _shroomType.name == shroomType.name;

        public void ConnectChild(ShroomBase shroom)
        {
            this.Children.Add(shroom);
            shroom.Parent = this;
            // TODO connect via roots
        }

        // Feel free to override this method for specific Shrooms
        public void Initialize()
        {
            this._shroomId = MushroomManager.Instance.GenerateUniqueId();
            this.Children = new List<ShroomBase>();
            this.transform.position = new Vector3(_shroomPosition.x, _shroomPosition.y, 1);
            _connector = GetComponent<LineRenderer>();
            _connector.SetPositions(new Vector3[] {transform.position, transform.position});

            if (Parent != null)
            {
                _connector.SetPosition(1, new Vector3(ParentPosition.x, ParentPosition.y, 1));
            }
        }

        public abstract void Start();

        public abstract void Update();

        public override string ToString()
        {
            return ShroomType + "{" +
                   "\n ID=" + _shroomId +
                   "\n Position=" + _shroomPosition +
                   "\n Parent=" + _parent + "\n}";
            // "\n Children=" + String.Join('\n', _children) + "\n}";
        }
    }
}
