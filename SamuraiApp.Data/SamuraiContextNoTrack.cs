using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.Data
{
    public class SamuraiContextNoTrack : SamuraiContext
    {
        public SamuraiContextNoTrack()
        {
            base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}