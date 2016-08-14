using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class Touchpad_Target_CS : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public string horizontalAxisName = "Target_X"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Target_Y"; // The name given to the vertical axis for the cross platform input

		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		bool m_Dragging;
		int m_Id = -1;

		
		void OnEnable() {
			CreateVirtualAxes();
		}
		
		void CreateVirtualAxes () {
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}
		
		void UpdateVirtualAxes ( Vector3 value ) {
			m_HorizontalVirtualAxis.Update(value.x);
			m_VerticalVirtualAxis.Update(value.y);
		}
		
		
		public void OnPointerDown(PointerEventData data)
		{
			m_Dragging = true;
			m_Id = data.pointerId;
		}
		
		void Update () {
			if  ( !m_Dragging ) {
				return;
			}
			if ( Input.touchCount >= m_Id + 1 && m_Id != -1 ) {
				Vector3 touchPos ;
				#if !UNITY_EDITOR
				touchPos = new Vector3 ( Input.touches[m_Id].position.x , Input.touches[m_Id].position.y , 0.0f ) ;
				#else
				touchPos = Input.mousePosition ;
				#endif
				UpdateVirtualAxes ( touchPos );
			}
		}
		
		
		public void OnPointerUp ( PointerEventData data ) {
			m_Dragging = false;
			m_Id = -1;
		}
		
		void OnDisable () {
			if ( CrossPlatformInputManager.AxisExists ( horizontalAxisName ) )
				CrossPlatformInputManager.UnRegisterVirtualAxis ( horizontalAxisName ) ;
			
			if ( CrossPlatformInputManager.AxisExists ( verticalAxisName ) )
				CrossPlatformInputManager.UnRegisterVirtualAxis ( verticalAxisName ) ;
		}
	}
}