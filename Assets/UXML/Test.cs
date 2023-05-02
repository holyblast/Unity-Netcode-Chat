using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset chatUI;

    private void OnEnable()
    {
        TemplateContainer chatrow = chatUI.Instantiate();
    }

    public void SendChat()
    {

    }
}
