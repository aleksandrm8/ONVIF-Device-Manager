using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using utils;

namespace odm.ui.views
{
    public abstract class VAEntity
    {
        public abstract bool Fit(VAEntitySnapshot snapshot);
        public abstract void Update(VAEntitySnapshot snapshot, Func<double, double> scaleX, Func<double, double> scaleY);

        public DateTime LastUpdated { get; private set; }
        public event EventHandler Updated;
        protected void FireUpdated()
        {
            LastUpdated = DateTime.Now;
            if (this.Updated != null)
                this.Updated(this, EventArgs.Empty);
        }
    }

    public abstract class VAAlarm : VAEntity
    {
        public virtual string Name { get { return GetType().Name; } }
        public virtual bool State { get; set; }
    }

    [Serializable]
    public abstract class VAEntitySnapshot
    {
        public DateTime Time { get; set; }
        public abstract VAEntity Create();
    }

    public class VAEntitiesHolder<TEntity, TSnapshot>
        where TEntity : VAEntity
        where TSnapshot : VAEntitySnapshot
    {
        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        
        public readonly ObservableCollection<TEntity> Entities = new ObservableCollection<TEntity>(); // TODO : readonly collection
        
        
        readonly Func<double, double> scaleX = (x) => x;
        readonly Func<double, double> scaleY = (y) => y;

        readonly DispatcherTimer gcTimer;

        public VAEntitiesHolder(Func<double, double> scaleX, Func<double, double> scaleY, Dispatcher dispatcher)
        {
            if (scaleX != null)
                this.scaleX = scaleX;
            if (scaleY != null)
                this.scaleY = scaleY;
            if (dispatcher != null)
                this.dispatcher = dispatcher;

            gcTimer = new DispatcherTimer(TimeSpan.FromSeconds(maxTime_secs / 2.0), DispatcherPriority.Normal, gcTimer_Tick, this.dispatcher);
            gcTimer.Start();
        }

        const int maxTime_secs = 5;
        private void gcTimer_Tick(object sender, EventArgs e)
        {
            //remove hanging entities (those which hasn't received Deleted event)
            for (int i = 0; i < this.Entities.Count; ++i)
            {
                var entity = this.Entities[i];
                if ((DateTime.Now - entity.LastUpdated) > TimeSpan.FromSeconds(maxTime_secs))
                {
                    this.Entities.RemoveAt(i);
                }
            }
        }

        public void EntityDeleted(TSnapshot snapshot)
        {
            dispatcher.BeginInvoke(new Action(delegate 
                {
                    for (int i = Entities.Count - 1; i >= 0; i--)
                    {
                        if (Entities[i].Fit(snapshot))
                        {
                            Entities.RemoveAt(i);
                            break;
                        }
                    }
                }));
        }

        public void EntityChanged(TSnapshot snapshot)
        {
            dispatcher.BeginInvoke(new Action(delegate
            {
                bool exists = false; //TDOO 
                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i].Fit(snapshot))
                    {
                        Entities[i].Update(snapshot, scaleX, scaleY); 
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    var newEntity = (TEntity)snapshot.Create();
                    newEntity.Update(snapshot, scaleX, scaleY);
                    Entities.Add(newEntity);
                }
            }));
        }

        public void EntityInitialized(TSnapshot snapshot)
        {
            dispatcher.BeginInvoke(new Action(delegate
            {
                bool exists = false;
                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i].Fit(snapshot))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    var newEntity = (TEntity)snapshot.Create();
                    newEntity.Update(snapshot, scaleX, scaleY);
                    Entities.Add(newEntity);
                }
            }));
        }
    }
}
