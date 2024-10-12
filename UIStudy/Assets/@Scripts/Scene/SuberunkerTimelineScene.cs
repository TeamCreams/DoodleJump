using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SuberunkerTimelineScene : BaseScene
{
    PlayableDirector _playableDirector;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        GameObject gameObject = Instantiate(Managers.Resource.Load<GameObject>("SuberunkerTimeline"));
        _playableDirector = gameObject.GetOrAddComponent<PlayableDirector>();
        _playableDirector.name = "SuberunkerTimeline";
        _playableDirector.Play();
        gameObject.GetOrAddComponent<CameraAnimationEvents>().SetInfo(1);


        return true;
    }


}