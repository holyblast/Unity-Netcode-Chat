using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public static Action<string, string> ReceiveMessageEvent;

    #region Screens

    private VisualElement m_Home;
    private VisualElement m_Chat;

    #endregion

    private TextField m_message;

    [SerializeField] private VisualTreeAsset m_TreeAsset;

    private void OnEnable()
    {
        ReceiveMessageEvent += ReceiveMessage;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        //Assign Screens
        
        m_Home = root.Q("Test");
        m_Chat = root.Q("ChatUI");


        m_Home.Q("Host")?.RegisterCallback<ClickEvent>(StartHost);
        m_Home.Q("Server")?.RegisterCallback<ClickEvent>(StartServer);
        m_Home.Q("Client")?.RegisterCallback<ClickEvent>(StartClient);

        m_message = (TextField)m_Chat.Q("Message");
        m_Chat.Q("SendMessage").RegisterCallback<ClickEvent>(ValidateMessage);
    }

    private void OnDisable()
    {
        ReceiveMessageEvent -= ReceiveMessage;
    }

    private void DisableAllScreens()
    {
        m_Home.style.display = DisplayStyle.None;
        m_Chat.style.display = DisplayStyle.None;
    }

    private void StartHost(ClickEvent evt)
    {
        NetworkManager.Singleton.StartHost();

        DisableAllScreens();
        m_Chat.style.display = DisplayStyle.Flex;
    }
    private void StartServer(ClickEvent evt)
    {
        NetworkManager.Singleton.StartServer();

        DisableAllScreens();
    }

    private void StartClient(ClickEvent evt)
    {
        NetworkManager.Singleton.StartClient();

        DisableAllScreens();
        m_Chat.style.display = DisplayStyle.Flex;
    }

    private void ValidateMessage(ClickEvent evt)
    {
        if (m_message.text == "") return;

        Player.SendMessageEvent?.Invoke(m_message.text);
        m_message.SetValueWithoutNotify("");
    }

    private void ReceiveMessage(string sender, string message)
    {
        TemplateContainer chatrow = m_TreeAsset?.Instantiate();
        ((Label)chatrow.Q("UserInfo")).text = sender;
        ((Label)chatrow.Q("Message")).text = message;
        m_Chat.Q("TextArea")?.Add(chatrow);
    }
}
