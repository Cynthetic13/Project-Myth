using System;
using Steamworks;
using UnityEngine;

namespace Myth.Core
{
    public class SteamScript : MonoBehaviour
    {
        public string Name;
        
        private void Awake()
        {
            if (SteamManager.Initialized)
            {
                Name = SteamFriends.GetPersonaName();
            }
        }

        private void OnDestroy()
        {
            SteamAPI.Shutdown();
        }
    }
}