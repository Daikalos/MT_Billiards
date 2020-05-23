using System;
using System.Drawing;
using System.Windows.Forms;

namespace Multithreading_06
{
    internal static class Extensions
    {
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static float Length(PointF point)
        {
            return (float)Math.Sqrt((Math.Pow(point.X, 2) + Math.Pow(point.Y, 2)));
        }

        public static PointF Normalize(this PointF point)
        {
            float distance = Length(point);
            return (distance > 1.0f) ? new PointF(point.X / distance, point.Y / distance) : PointF.Empty;
        }

        public static PointF Multiply(this PointF point, Point pointToMultiplyBy)
        {
            return new PointF(point.X * pointToMultiplyBy.X, point.Y * pointToMultiplyBy.Y);
        }

        public static PointF MultiplyValue(this PointF point, float value)
        {
            return new PointF(point.X * value, point.Y * value);
        }

        public static PointF Add(this PointF point, PointF pointToAddBy)
        {
            return new PointF(point.X + pointToAddBy.X, point.Y + pointToAddBy.Y);
        }

        public static PointF Subtract(this PointF point, PointF pointToSubtractBy)
        {
            return new PointF(point.X - pointToSubtractBy.X, point.Y - pointToSubtractBy.Y);
        }

        public static float Clamp(this float value, float min, float max)
        {
            value = (value < min) ? min : value;
            value = (value > max) ? max : value;

            return value;
        }

        public static bool WithinBall(Ball ball, PointF point)
        {
            PointF ballCenter = ball.Position;
            float ballRadians = (ball.Size.Width / 2);

            float distance = Length(ballCenter.Subtract(point));

            if (distance <= ballRadians)
            {
                return true;
            }

            return false;
        }

        public static bool CheckBallCollision(Ball firstBall, Ball secondBall)
        {
            PointF firstBallCenter = firstBall.Position.Add(firstBall.Velocity);
            float firstBallRadians = (firstBall.Size.Width / 2);

            PointF secondBallCenter = secondBall.Position.Add(secondBall.Velocity);
            float secondBallRadians = (secondBall.Size.Width / 2);

            float tempDistance = (float)Math.Sqrt(
                Math.Pow(firstBallCenter.X - secondBallCenter.X, 2) +
                Math.Pow(firstBallCenter.Y - secondBallCenter.Y, 2));

            if (tempDistance <= firstBallRadians + secondBallRadians)
            {
                return true;
            }

            return false;
        }

        public static bool CheckHoleCollision(Ball ball, Hole hole)
        {
            PointF ballCenter = ball.Position;
            float ballRadians = (ball.Size.Width / 2);

            PointF holeCenter = hole.Position;
            float holeRadians = (hole.Size.Width / 2);

            float tempDistance = Length(ballCenter.Subtract(holeCenter));

            if (tempDistance <= ballRadians + holeRadians)
            {
                return true;
            }

            return false;
        }
    }
}