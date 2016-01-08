using System;


namespace GithubActors.Messages.GithubRepositoryAnalysis
{
    public class RetryableQuery
    {
        public RetryableQuery(object query, int allowableTries)
            : this(query, allowableTries, 0)
        {
        }


        private RetryableQuery(object query, int allowableTries, int currentAttempt)
        {
            AllowableTries = allowableTries;
            Query = query;
            CurrentAttempt = currentAttempt;
        }


        public int AllowableTries { get; private set; }

        public bool CanRetry
        {
            get { return RemainingTries > 0; }
        }

        public int CurrentAttempt { get; private set; }
        public object Query { get; private set; }

        public int RemainingTries
        {
            get { return AllowableTries - CurrentAttempt; }
        }


        public RetryableQuery NextTry()
        {
            return new RetryableQuery(Query, AllowableTries, CurrentAttempt + 1);
        }
    }
}
