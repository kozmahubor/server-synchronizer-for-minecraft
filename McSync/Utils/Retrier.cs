﻿using System;

namespace McSync.Utils
{
    public class Retrier
    {
        public void RetryUntilThrowsNoException(Action retryableAction, Action<Exception> actionOnException)
        {
            bool isSuccessful = false;
            while (!isSuccessful)
                try
                {
                    retryableAction();
                    isSuccessful = true;
                }
                catch (Exception e)
                {
                    actionOnException(e);
                }
        }
    }
}