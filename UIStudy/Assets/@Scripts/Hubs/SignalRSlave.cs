using System.Threading.Tasks;
using UnityEngine;

public class SignalRSlave : MonoBehaviour
{
    private string _nickname;
    private string _message;
    private void OnDisable()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
    public async Awaitable HandleReceiveMessage(string nickname, string message)
    {
        Debug.Log($"HandleReceiveMessage [ {nickname} ]  {message}");
        _nickname = nickname;
        _message = message;
        var result = await ReceiveMessageAsync();
       //await Awaitable.BackgroundThreadAsync();
        if(result)
        {   
            Debug.Log($"HandleReceiveMessage Complete");
        }
    }
    private async Awaitable<bool> ReceiveMessageAsync()
    {
        // 나는 메인스레드가 날 잡아줄때까지 기다릴거야.
        await Awaitable.MainThreadAsync();
        bool isComplete = false;
        //Debug.Log($"ReceiveMessageCo :  [ {_nickname} ]  {_message}"); // 안들어옴
        Managers.Game.ChattingInfo.SenderNickname = _nickname;

        //Managers.Event.TriggerEvent(Define.EEventType.ReceiveMessage);        
        ChattingStruct chattingStruct = Managers.Chatting.GetChattingStruct();
        var bubble = Managers.UI.MakeSubItem<UI_ChattingItem>(parent: Managers.Game.ChattingInfo.Root);
        if (bubble == null)
        {
            Debug.Log("Bubble이 생성되지 않음.");
        }
        else
        {
            Debug.Log("Bubble이 성공적으로 생성됨.");
            isComplete = true;
            bubble.SetInfo(chattingStruct);
        }
        Managers.Resource.Destroy(this.gameObject);
        return isComplete;
    }
}