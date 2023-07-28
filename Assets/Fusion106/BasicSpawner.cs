using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fusion106
{
    public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner;
        public bool autoJoin = false;
        public bool autoSingleJoin = false;
        public InputTest inputTest;
        public int waitTime;
        [Networked] public NetworkButtons ButtonsPrevious { get; set; }


        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
                {
                    StartGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
                {
                    StartGame(GameMode.Client);
                }
                if (GUI.Button(new Rect(0, 80, 200, 40), "Join as spectator"))
                {
                    StartGame(GameMode.AutoHostOrClient);
                }
                if (GUI.Button(new Rect(0, 120, 200, 40), "Single Mode"))
                {
                    StartGame(GameMode.Single);
                }
            }
        }

        private void Start()
        {

            Invoke("AutoJoinRoom", waitTime);
            Invoke("AutoSingleJoinRoom", waitTime);

        }



        private void AutoJoinRoom()
        {
            if (autoJoin)
            {
                StartGame(GameMode.AutoHostOrClient);
            }
        }
        private void AutoSingleJoinRoom()
        {
            if (autoSingleJoin)
            {
                StartGame(GameMode.Single);
            }
        }

        async void StartGame(GameMode mode)
        {



            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(
                new StartGameArgs()
                {
                    GameMode = mode,
                    SessionName = "TestRoom",
                    Scene = SceneManager.GetActiveScene().buildIndex,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                }
            );
        }

        [SerializeField]
        private NetworkPrefabRef _playerPrefab; // Character to spawn for a joining player
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters =
            new Dictionary<PlayerRef, NetworkObject>();

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                Vector3 spawnPosition = new Vector3(
                    (player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3,
                    1,
                    0
                );
                NetworkObject networkPlayerObject = runner.Spawn(
                    _playerPrefab,
                    spawnPosition,
                    Quaternion.identity,
                    player
                );
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        private bool _mouseButton0;
        private bool _mouseButton1;

        private void Update()
        {
            _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
            _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
            
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            
            var data = new NetworkInputData();

            data.directionFromLeftStick = new Vector3(inputTest.leftStick.x, 0, inputTest.leftStick.y);
            data.directionFromRightStick = new Vector3(inputTest.rightStick.x, 0, inputTest.rightStick.y);
            if (Input.GetKey(KeyCode.W))
                data.direction += Vector3.forward;

            if (Input.GetKey(KeyCode.S))
                data.direction += Vector3.back;

            if (Input.GetKey(KeyCode.A))
                data.direction += Vector3.left;

            if (Input.GetKey(KeyCode.D))
                data.direction += Vector3.right;

            if (_mouseButton0)
                data.buttons |= NetworkInputData.MOUSEBUTTON1;
            _mouseButton0 = false;

            if (_mouseButton1)
                data.buttons |= NetworkInputData.MOUSEBUTTON2;
            _mouseButton1 = false;

            if (MyStaticClass.Variable1 != null)
            {
                data.targetCubeRotation = MyStaticClass.Variable1;
            }

            // if (MyStaticClass.bodyTorque != null)
            // {
            //     Debug.Log("bodyTorque IN BASIC SPAWNER");
            //     data.bodyTorque = MyStaticClass.bodyTorque;
            //     data.headTorque = MyStaticClass.headTorque;
            //     data.leftArmTorque = MyStaticClass.leftArmTorque;
            //     data.rightArmTorque = MyStaticClass.rightArmTorque;
            //     data.leftForearmTorque = MyStaticClass.leftForearmTorque;
            //     data.rightForearmTorque = MyStaticClass.rightForearmTorque;
            //     data.leftHandTorque = MyStaticClass.leftHandTorque;
            //     data.rightHandTorque = MyStaticClass.rightHandTorque;
            // }

            data.bodyTorque = MyStaticClass.bodyTorque;
            data.headTorque = MyStaticClass.headTorque;
            data.leftArmTorque = MyStaticClass.leftArmTorque;
            data.rightArmTorque = MyStaticClass.rightArmTorque;
            data.leftForearmTorque = MyStaticClass.leftForearmTorque;
            data.rightForearmTorque = MyStaticClass.rightForearmTorque;
            data.leftHandTorque = MyStaticClass.leftHandTorque;
            data.rightHandTorque = MyStaticClass.rightHandTorque;

            data.headDirection = MyStaticClass.headDirection;

            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnDisconnectedFromServer(NetworkRunner runner) { }

        public void OnConnectRequest(
            NetworkRunner runner,
            NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token
        )
        { }

        public void OnConnectFailed(
            NetworkRunner runner,
            NetAddress remoteAddress,
            NetConnectFailedReason reason
        )
        { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnCustomAuthenticationResponse(
            NetworkRunner runner,
            Dictionary<string, object> data
        )
        { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnReliableDataReceived(
            NetworkRunner runner,
            PlayerRef player,
            ArraySegment<byte> data
        )
        { }

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}
