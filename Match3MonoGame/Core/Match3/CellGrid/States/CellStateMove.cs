using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;

namespace Match3MonoGame.Core.Match3.CellGrid.States
{
    public class CellStateMove : State<CellFiniteStateMachine>
    {
        public CellStateMove(CellFiniteStateMachine fsm) : base(fsm)
        {
            CellStateSemaphore.MoveCellCount++;
        
        }


        public override void Process(GameTime gameTime)
        {
            if (CellStateSemaphore.DestroySemaphore > 0)
                return;

            var fsm = GetFsm();
            var cell = fsm.GetCell();
            var grid = cell.GetGrid();
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var pos = grid.GetPositionCell(cell.X, cell.Y);
            var selfPos = cell.Position;
            var distance = Vector2.Distance(selfPos, pos);
            if (distance > CellFiniteStateMachine.MinDistance)
            {
                var direction = Vector2.Normalize(pos - selfPos);
                var velocity = direction * cell.FallSpeed * delta;
                if (velocity.Length() > distance)
                {
                    velocity.Normalize();
                    velocity *= distance;
                }
                cell.Position += velocity;
            }
            else
            {
                GetFsm().PopState();
                cell.LastMovedTimestamp = gameTime.TotalGameTime.TotalMilliseconds;
                CellStateSemaphore.MoveCellCount--;
            }
        }
    }
}
