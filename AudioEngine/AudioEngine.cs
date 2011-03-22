using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.AudioEngine
{
    class AudioEngine
    {
        private BaseModel baseModel;

        public AudioEngine(BaseModel baseModel)
        {
            this.baseModel = baseModel;
        }

        public AudioSummary Tick(int barIndicator)
        {
            return null;
        }
    }
}
