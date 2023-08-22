using Fusion;
using UnityEngine;


namespace Fusion107
{
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
    {
        public GameObject PlayerPrefab;
        private NetworkRunner _runner;

        private void Awake()
        {
            _runner = GetComponent<NetworkRunner>();
        }

        public void PlayerJoined(PlayerRef player)
        {
            Vector3 spawnPosition = new Vector3(
                    (player.RawEncoded % _runner.Config.Simulation.DefaultPlayers) * 3,
                    1,
                    0
                );
            if (player == Runner.LocalPlayer)
            {
                Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
            }
            //_runner.TryFindObject(player, out PlayerController playerController);
        }
    }


}