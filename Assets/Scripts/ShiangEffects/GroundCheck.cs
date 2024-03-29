
using UnityEngine;

namespace Shiang
{
    /// <summary>
    /// Check if the player is grounded and 
    /// recognise the ground type by line raycasting.
    /// </summary>
    /// 
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] Transform _endPoint;
        [SerializeField] LayerMask _layerMask;
        public bool IsGrounded { get; private set; }
        public GroundType CurrentGroundType { get; private set; }

        // Update is called once per frame
        void Update()
        {
            RaycastHit2D hit = Physics2D.Linecast(
                transform.position, 
                _endPoint.position, 
                _layerMask);

            IsGrounded = hit;

#if UNITY_EDITOR
            Debug.DrawRay(
                transform.position,
                Vector2.up * _endPoint.localPosition.y,
                Color.green);
#endif

            if (IsGrounded)
            {
                Ground ground = hit.transform.gameObject.GetComponent<Ground>();
                if (ground != null) CurrentGroundType = ground.Type;
                else CurrentGroundType = GroundType.None;
            }
            else
            {
                CurrentGroundType = GroundType.None;
            }
        }
    }
}