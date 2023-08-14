using Fusion;
using UnityEngine;



namespace Fusion107
{
    public class Health : NetworkBehaviour
    {
        [Networked]
        public float NetworkedHealth { get; set; } = 100;


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void DealDamageRpc(float damage)
        {
            // The code inside here will run on the client which owns this object (has state and input authority).
            Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
            NetworkedHealth -= damage;
        }
    }



}