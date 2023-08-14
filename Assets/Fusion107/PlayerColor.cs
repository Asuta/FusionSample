using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


namespace Fusion107
{
    public class PlayerColor : NetworkBehaviour
    {
        public MeshRenderer MeshRenderer;

        [Networked(OnChanged = nameof(NetworkColorChanged))]
        public Color NetworkedColor { get; set; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
                NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }
        }

        private static void NetworkColorChanged(Changed<PlayerColor> changed)
        {
            changed.Behaviour.MeshRenderer.material.color = changed.Behaviour.NetworkedColor;
        }
    }
}