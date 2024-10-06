using Microsoft.Xna.Framework;

namespace MonoZenith
{
    public class Timer
    {
        public readonly float OriginalSeconds; // Original time in seconds
        private float _remainingSeconds; // Remaining time in seconds

        public Timer(float seconds)
        {
            OriginalSeconds = seconds;
            _remainingSeconds = seconds;
        }

        /// <summary>
        /// Update the timer by subtracting the elapsed game time from the remaining time.
        /// </summary>
        /// <param name="deltaTime">The game time (in seconds).</param>
        public void Update(GameTime deltaTime)
        {
            if (_remainingSeconds <= 0)
                return;

            _remainingSeconds -= (float)deltaTime.ElapsedGameTime.TotalSeconds;
            if (_remainingSeconds < 0)
            {
                _remainingSeconds = 0;
            }
        }

        /// <summary>
        /// Check if the timer has run out.
        /// </summary>
        /// <returns>Whether the timer has finished (true if time is up).</returns>
        public bool TimerOver()
        {
            return _remainingSeconds <= 0;
        }

        /// <summary>
        /// Reset the timer to its original duration.
        /// </summary>
        public void ResetTimer()
        {
            _remainingSeconds = OriginalSeconds;
        }

        /// <summary>
        /// Get the progress of the timer as a percentage (between 0 and 1).
        /// </summary>
        /// <returns>Progress as a float (0 to 1).</returns>
        public float GetProgress()
        {
            return 1 - (_remainingSeconds / OriginalSeconds);
        }

        /// <summary>
        /// Get the remaining time in seconds.
        /// </summary>
        /// <returns>Remaining time as a float (in seconds).</returns>
        public float GetRemainingTime()
        {
            return _remainingSeconds;
        }

        /// <summary>
        /// Check if the timer is currently active (i.e., still counting down).
        /// </summary>
        /// <returns>True if the timer is still running, false if it has finished.</returns>
        public bool IsActive()
        {
            return _remainingSeconds > 0;
        }
    }
}