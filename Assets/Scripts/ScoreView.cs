using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour, ISubscriber
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] string scorePrefix = "Score: ";
    [SerializeField] float currentScore = 0;

    MessageBroker globalMsgBroker;

    private void Start()
    {
        if (ServiceLocator.RequestService("messageBroker", out IService outService))
            globalMsgBroker = (MessageBroker)outService;

        globalMsgBroker.RegisterSubscriber("cropSold", this);
    }

    void UpdateScore(float score)
    {
        currentScore += score;
        scoreText.text = scorePrefix + currentScore.ToString();
    }

    public bool Receive(Message msg)
    {
        if (msg.MessageType == "cropSold")
        {
            UpdateScore(100.0f);
        }

        return false;
    }
}