using UnityEngine;

namespace IA
{
    public class EventManager : MonoBehaviour
    {
        public EventController OnCarModelEvent { get; private set; }
        public EventController OnPlaneModelEvent { get; private set; }

        public EventsController<ModelType> OnModelSelectedEvent { get; private set; }

        public EventManager()
        {
            OnCarModelEvent = new EventController();
            OnPlaneModelEvent = new EventController();
            OnModelSelectedEvent = new EventsController<ModelType>();
        }
    }
}
