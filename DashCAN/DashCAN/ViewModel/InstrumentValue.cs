using DashCAN.Common;
using Microsoft.UI.Dispatching;

namespace DashCAN.ViewModel
{
    public abstract class InstrumentValue : ViewModelBase
    {
        public InstrumentValue(Unit displayUnit, DataValue dataValue)
        {
            DisplayUnit = displayUnit;
            DataValue = dataValue;
            DispatcherQueue = DispatcherQueue.GetForCurrentThread();
            DataValue.ValueChanged += DataValue_ValueChanged;
        }

        public Unit DisplayUnit { get; private set; }
        private readonly DataValue DataValue;
        private readonly DispatcherQueue DispatcherQueue;

        protected abstract void SetValue(DataValue value);

        private void DataValue_ValueChanged(object? sender, EventArgs e)
        {
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                SetValue(DataValue);
            });
        }
    }
}
