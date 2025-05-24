// 로그인 관련 정보를 관리하는 비 MonoBehaviour 클래스
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using static Define;

public class LoginSlave : MonoBehaviour
{
    private bool _isLoadEnergyCondition = false;
    private int _failCount = 0;

    // 에너지 업데이트 시작
    public void StartUpdateEnergy()
    {
        _isLoadEnergyCondition = true;
        StartCoroutine(UpdateEnergy());
    }

    // 에너지 업데이트 코루틴
    private IEnumerator UpdateEnergy()
    {
        yield return new WaitWhile(() => _isLoadEnergyCondition == false);

        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.UpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Debug.Log("에너지 업데이트 성공: " + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;

            // 로그인 완료 후 씬 로드
            //Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            UI_ToastPopup.ShowInfo(Managers.Error.GetError(EErrorCode.ERR_NetworkLoginSuccess), 2f, () => Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene));
        },
        (errorCode) =>
        {
            loadingComplete.Value = true;
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
            HandleFailure();
        });
    }

    // 에너지 업데이트 실패 처리
    private void HandleFailure()
    {
        if (_failCount < HardCoding.MAX_FAIL_COUNT)
        {
            _failCount++;
            StartCoroutine(UpdateEnergy());
            return;
        }
        _failCount = 0;
        Managers.Scene.LoadScene(EScene.StartLoadingScene);
    }
}