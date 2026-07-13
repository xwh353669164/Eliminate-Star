using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    public Button btnClose;
    public Button btnRegister;
    public InputField userNameInput;
    public InputField passwordInput;

    public override void Init()
    {
        userNameInput.contentType = InputField.ContentType.Alphanumeric;
        passwordInput.contentType = InputField.ContentType.Password;

        btnClose.onClick.AddListener(CloseSelf);
        btnRegister.onClick.AddListener(() =>
        {
            AccountResult result = LocalDataService.Instance.Register(userNameInput.text, passwordInput.text);
            if (result != AccountResult.Success)
            {
                ShowMessage(GetMessage(result));
                return;
            }

            CloseSelf();
        });
    }

    public override void UnInit()
    {
        btnClose.onClick.RemoveAllListeners();
        btnRegister.onClick.RemoveAllListeners();
        userNameInput.onEndEdit.RemoveAllListeners();
        passwordInput.onEndEdit.RemoveAllListeners();
    }

    private void CloseSelf()
    {
        UIManager.Instance.HidePanel<RegisterPanel>();
        UIManager.Instance.ShowPanel<LoadPanel>();
    }

    private void ShowMessage(string message)
    {
        passwordInput.text = string.Empty;
        passwordInput.placeholder.GetComponent<Text>().text = message;
    }

    private static string GetMessage(AccountResult result)
    {
        switch (result)
        {
            case AccountResult.InvalidFormat:
                return "账号和密码只能使用字母或数字";
            case AccountResult.AccountExists:
                return "账号已存在";
            default:
                return string.Empty;
        }
    }
}
