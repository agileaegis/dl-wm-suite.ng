using System.Threading.Tasks;
using dl.wm.models.DTOs.Users;

namespace dl.wm.view.Controls.LoginForget
{
    public interface IAuthJwtView : IView
    {
        string OnMessageError { get; set; }
        AuthUiModel JwtAuthUiModel { get; set; }
        string OnBadRequestForUserJwtResult { set; }
    }
}
