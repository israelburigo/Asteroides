using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroides.Engine
{
    public class Animation
    {
        public bool Loop { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public float[] TimePerFrame { get; set; }
        public bool Started { get; private set; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public Texture2D Texture { get; }       
        public Rectangle Source { get; private set; }   
        public int Rows { get; }
        public int Cols { get; }
        public int Frames { get; }        
        private int _index;    
        private float _timeInFrame;
        private int _timeIndex;
        public Vector2 Center { get; private set; }
        
        public Animation(Texture2D texture, int rows, int cols, int frames)
        {
            Texture = texture;
            Rows = rows;
            Cols = cols;
            Frames = frames;
            FrameWidth = texture.Width / cols;
            FrameHeight = texture.Height / rows;
            StartIndex = 0;        
            EndIndex = frames - 1;
            TimePerFrame = new [] { 1f };
            _index = StartIndex;
            _timeIndex = 0;
            _timeInFrame = TimePerFrame[_timeIndex];        
        }

        public Animation Start()
        {
            if(Started)
                return this;
            Started = true;
            _index = StartIndex;
            _timeIndex = 0;
            _timeInFrame = TimePerFrame[_timeIndex];        
            return this;
        }

        public Animation Stop()
        {
            if(!Started)
                return this;

            Started = false;
            return this;
        }

        public Animation SelectIndex(int index)
        {
            _index = index;
            var x = (_index % Cols) * FrameWidth;
            var y = (_index / Cols) * FrameHeight;
            Source = new Rectangle(x, y, FrameWidth, FrameHeight);
            Center = new Vector2(FrameWidth / 2, FrameHeight / 2);
            return this;
        }

        public void Update(GameTime gt)
        {
            if(!Started)
                return;

            var dt = gt.GetDelta();

            if((_timeInFrame -= dt) > 0)
                return;
            
            if(++_timeIndex >= TimePerFrame.Length)
                _timeIndex = 0;

            _timeInFrame = TimePerFrame[_timeIndex];

            var x = (_index % Cols) * FrameWidth;
            var y = (_index / Cols) * FrameHeight;

            Source = new Rectangle(x, y, FrameWidth, FrameHeight);
            Center = new Vector2(FrameWidth / 2, FrameHeight / 2);

            if(++_index > EndIndex)
                _index = Loop ? StartIndex : EndIndex;

        }
    }
}