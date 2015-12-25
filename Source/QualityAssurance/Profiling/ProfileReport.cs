using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public class ProfileReport<TPayload>
    {
        private static readonly int[] g_percentages = new[] { 50, 66, 75, 80, 90, 95, 98, 99 };

        public ProfileReport(TimeSpan elapsed, IEnumerable<ProfileResult<TPayload>> profileResults)
        {
            var results = profileResults.OrderBy(r => r.Elapsed.Ticks).ToArray();
            if (results.Length == 0)
            {
                throw new ArgumentOutOfRangeException("profileResults", "profileResults has zero length");
            }

            Elapsed = elapsed;
            Succeed = results.All(r => r.Succeed);
            if (!Succeed)
            {
                var r = results.Where(x => !x.Succeed).First();
                Error = r.Error;
                ErrorPayload = r.Payload;
            }

            var totalTicks = results.Sum(r => r.Elapsed.Ticks);

            RequestsServed = results.Length;
            FailedRequests = results.Count(r => !r.Succeed);
            AverageTime = new TimeSpan(totalTicks / results.Length);
            MinTime = new TimeSpan(results.Min(r => r.Elapsed.Ticks));
            MaxTime = new TimeSpan(results.Max(r => r.Elapsed.Ticks));
            if (elapsed.TotalMilliseconds > 0.0)
            {
                Throughtput = results.Length * 1000.0 / elapsed.TotalMilliseconds;
            }

            Results = results;
        }

        public bool Succeed { get; private set; }

        public Exception Error { get; private set; }

        public TPayload ErrorPayload { get; private set; }

        public int RequestsServed { get; private set; }

        public int FailedRequests { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public ProfileResult<TPayload>[] Results { get; private set; }

        public TimeSpan MinTime { get; private set; }

        public TimeSpan AverageTime { get; private set; }

        public TimeSpan MaxTime { get; private set; }

        public double Throughtput { get; private set; }

        public IEnumerable<KeyValuePair<int, TimeSpan>> ServedWithin(int[] percentages)
        {
            return percentages.Select(p => new KeyValuePair<int, TimeSpan>(p, ServedWithin(p)));
        }

        public string ToShortString()
        {
            if (Throughtput > 1000)
            {
                return string.Format(CultureInfo.InvariantCulture, "Throughtput: {0:N}krps{1}", Throughtput / 1000, IfError());
            }

            return string.Format(CultureInfo.InvariantCulture, "Throughtput: {0:N}rps{1}", Throughtput, IfError());
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat(CultureInfo.InvariantCulture, "Requests served : {0}\r\n", RequestsServed)
                .AppendFormat(CultureInfo.InvariantCulture, "Failed requests : {0}\r\n", FailedRequests)                
                .AppendFormat(CultureInfo.InvariantCulture, "Min/Agv/Max     : {0}/{1}/{2}\r\n",
                        TimeSpanHelper.ToShortTimeString(MinTime),
                        TimeSpanHelper.ToShortTimeString(AverageTime),
                        TimeSpanHelper.ToShortTimeString(MaxTime))
                .AppendFormat(CultureInfo.InvariantCulture, "Time taken      : {0}\r\n", TimeSpanHelper.ToShortTimeString(Elapsed))
                .AppendFormat(CultureInfo.InvariantCulture, "Throughput      : {0:N}rps\r\n\r\n", Throughtput)
                .AppendFormat(CultureInfo.InvariantCulture, "Served within a certain time\r\n")
                .Append(StringHelper.Join("\r\n", ServedWithin(g_percentages)
                    .Select(p => String.Format(CultureInfo.InvariantCulture, "  {0}%  {1}", p.Key, TimeSpanHelper.ToShortTimeString(p.Value)))))
                .Append(IfError())
                .ToString();
        }

        private TimeSpan ServedWithin(int percentage)
        {
            return new TimeSpan(Results.Take(Results.Length * percentage / 100)
                .Max(r => r.Elapsed.Ticks));
        }

        private string IfError()
        {
            if (Succeed)
            {
                return string.Empty;
            }

            var errorReport = new StringBuilder()
                .AppendFormat("\r\n\r\nError: {0}\r\n", Error.Message)
                .AppendFormat(CultureInfo.InvariantCulture, "Error Payload: '{0}'\r\n\r\n", ErrorPayload);

            foreach (DictionaryEntry entry in Error.Data)
            {
                errorReport.AppendFormat(CultureInfo.CurrentCulture, "{0}: {1}\r\n", entry.Key, entry.Value);
            }

            errorReport.Append(Error);
            return errorReport.ToString();
        }
    }
}
