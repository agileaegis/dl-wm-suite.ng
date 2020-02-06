using ws.simulator.Views;

namespace ws.simulator.Presenters.Base
{
    public class BasePresenter<TV>
        where TV : IView 
    {
        public BasePresenter(TV view)
        {
            View = view;
        }

        protected TV View { get; private set; }
    }
}