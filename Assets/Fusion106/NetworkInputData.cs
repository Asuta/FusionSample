using Fusion;
using Unity.Mathematics;
using UnityEngine;

namespace Fusion106
{
	public struct NetworkInputData : INetworkInput
	{
		public const byte MOUSEBUTTON1 = 0x01;
		public const byte MOUSEBUTTON2 = 0x02;

		public byte buttons;
		public Vector3 direction;
		public Vector3 headDirection;
		public Vector3 directionFromLeftStick;
		public Vector3 directionFromRightStick;

		public Quaternion targetCubeRotation;
		
		public Vector3 bodyTorque;
		public Vector3 headTorque;
		public Vector3 leftArmTorque;
		public Vector3 rightArmTorque;
		public Vector3 leftForearmTorque;
		public Vector3 rightForearmTorque;
		public Vector3 leftHandTorque;
		public Vector3 rightHandTorque;



		

	}
}
