using System.Collections.Generic;
using Myth.Misc;
using Myth.World.Blocks;

namespace Myth.World
{
    public class TickManager : Singleton<TickManager>
    {
        private const int TICK_INTERVAL = 10;
        
        private int _frameCounter;
        private List<TickableBlock> _blocksToTick = new();

        private void Update()
        {
            _frameCounter++;

            if (_frameCounter % TICK_INTERVAL == 0 && _blocksToTick.Count > 0)
            {
                // Tick
            }
        }
        
        public void RegisterTickable(byte blockID)
        {
            // Register Tickable Block
        }

        private struct TickableBlock
        {
            
        }
    }
}