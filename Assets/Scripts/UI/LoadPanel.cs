using UnityEngine.UI;

public class LoadPanel : BasePanel
{
    public Button btnLoad;
    public Button btnRegister;
    public InputField userNameInput;
    public InputField passwordInput;

    public override void Init()
    {
        userNameInput.contentType = InputField.ContentType.Alphanumeric;
        passwordInput.contentType = InputField.ContentType.Password;

        btnLoad.onClick.AddListener(() =>
        {
            AccountResult result = LocalDataService.Instance.Login(userNameInput.text, passwordInput.text);
            if (result != AccountResult.Success)
            {
                ShowMessage(GetMessage(result));
                return;
            }

            UIManager.Instance.HidePanel<LoadPanel>();
        });

        btnRegister.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<LoadPanel>();
            UIManager.Instance.ShowPanel<RegisterPanel>();
        });
    }

    public override void UnInit()
    {
        btnLoad.onClick.RemoveAllListeners();
        btnRegister.onClick.RemoveAllListeners();
        userNameInput.onEndEdit.RemoveAllListeners();
        passwordInput.onEndEdit.RemoveAllListeners();
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
            case AccountResult.AccountNotFound:
                return "账号不存在";
            case AccountResult.PasswordIncorrect:
                return "密码错误";
            default:
                return string.Empty;
        }
    }
}
