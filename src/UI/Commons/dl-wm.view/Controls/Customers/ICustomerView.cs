using System;

namespace dl.wm.view.Controls.Customers
{
    public interface ICustomerView : IMsgView
    {
        //CustomerUiModel Customer { set; }
        Guid SelectedCustomerId { get; set; }
    }
}