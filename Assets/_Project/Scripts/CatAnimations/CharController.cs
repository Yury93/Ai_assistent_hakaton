using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity; 
using System; 
using System.Linq;
using System.Text;
using Spine;
using JetBrains.Annotations;

public class CharController : MonoBehaviour
{
    [SerializeField] public List <BlendAnimation> blendAnimations;
    [SerializeField] public List <BlendAnimation> eventAnimations; 
    [SerializeField] public List<string> allAnimation;
    [SerializeField] public Lipsink lipsink;
    [SerializeField] public SkeletonGraphic skeletonGraphic; 
 
    public float blendAlpha = 0.5f; // Значение прозрачности смешивания анимаций
   
    public float speed,speedMixLipsink;
     
    public float startSpeed;
    private List<TrackEntry>  trackEntries;
    public void Init()
    { 
        trackEntries = new List<TrackEntry>();
        foreach (var item in eventAnimations[0].animations)
        {
            allAnimation.Add(item);
        }
        foreach (var item in blendAnimations[0].animations)
        {
            allAnimation.Add(item); 
        }
       
        int i = 0;
        foreach(var item in allAnimation)
        { 
           
           var blendAnim  = blendAnimations[0].animations.Contains(item) ;
            if (blendAnim == false)
            {
                MixAnim(i, item);
            }
            else
                  skeletonGraphic.AnimationState.SetAnimation(i, item, true);
             
            i++;
        }
         
        MainContainer.instance.TextToSpeech.onGetSpeech += OnGetSpeech; 
        var idle = lipsink.keys[0];
        skeletonGraphic.AnimationState.SetAnimation(8, idle.key, false);
        foreach (var item in lipsink.keys)
        {
            item.delayAnim = speed;
        }
        var state = skeletonGraphic.AnimationState;

       
    }

    

    private void MixAnim(int i, string item)
    {
        TrackEntry trackEntry = skeletonGraphic.AnimationState.SetAnimation(i, item, false);
        trackEntries.Add(trackEntry);

        int nextIndex = (i + 1) % allAnimation.Count;  // Взять следующий индекс с учетом зацикливания вокруг массива
        skeletonGraphic.AnimationState.Data.SetMix(allAnimation[i], allAnimation[nextIndex], 0.2f);
    }

    private void Update()
    {
        BlendAnimation();
    }

   

    private void OnGetSpeech(string message)
    {
        StartCoroutine(CorStartLipsink( message));
    }
    IEnumerator CorStartLipsink(string message)
    {
      
        StringBuilder sb = new StringBuilder();
        sb.Append(message);
      var state =  skeletonGraphic.AnimationState;
        state.Data.DefaultMix = speedMixLipsink;
        while (sb.Length != 0)
        {
            var firstSymbol = sb.ToString().FirstOrDefault();
            var key = lipsink.GetKey(firstSymbol);
            //Debug.Log(firstSymbol);
            if(key != lipsink.keys[0])
            skeletonGraphic.AnimationState.SetAnimation(8, key.key, false);
            sb.Remove(0,1);

            if (key != lipsink.keys[0])
                yield return new WaitForSeconds(key.delayAnim);
            else
            {
                yield return new WaitForSeconds(0.07f);
            }

        }
       var idle = lipsink.keys[0];
        state.Data.DefaultMix = startSpeed;
        skeletonGraphic.AnimationState.SetAnimation(8, idle.key, false);
    }

    //public void SetLoopBlendAnimation(int blendIndex)
    //{
    //    trackEntries = new TrackEntry[eventAnimations[0].animations.Count + blendAnimations[blendIndex].animations.Count +1];
    //    int i = 0;
    //    foreach (var animation in blendAnimations[blendIndex].animations)
    //    {
    //        skeletonGraphic.AnimationState.SetAnimation(i, animation, true);
    //        i++;
    //    }
    //    AddEventAnimation(i); 
    //}
      
    
    //public void AddEventAnimation(int indexEventAnimation)
    //{
    //  var generalCount =  eventAnimations[0].animations.Count + blendAnimations[0].animations.Count ;
    //    for (int i = indexEventAnimation; i < generalCount; i++)
    //    {
    //        trackEntries[i] = skeletonGraphic.AnimationState.SetAnimation(i, eventAnimations[0].animations[i - blendAnimations[0].animations.Count], true);
    //    }

    //    // Установка коэффициента смешивания между анимациями
    //    for (int i = indexEventAnimation; i < generalCount; i++)
    //    {
    //        int nextIndex = (i + 1) % eventAnimations[0].animations.Count;
    //        if (trackEntries[i] != null && trackEntries[nextIndex] != null)
    //        {
    //            skeletonGraphic.AnimationState.Data.SetMix(eventAnimations[0].animations[i - blendAnimations[0].animations.Count], eventAnimations[0].animations[nextIndex - blendAnimations[0].animations.Count], 0.2f);
    //        }
    //    }
    //}

    private void BlendAnimation()
    {
        // Установка прозрачности анимаций

        for (int i = 0; i < eventAnimations[0].animations.Count ; i++)
        {
            if (trackEntries[i] != null)
            {
                trackEntries[i].MixDuration = 0.2f;
                trackEntries[i].Alpha = blendAlpha;
            }
        }
    }
}
[Serializable]
public class BlendAnimation
{
    [SpineAnimation] public List<string> animations;
}
[Serializable]
public class Lipsink
{
    [SerializeField] public List<KeyAnimation> keys;


    public KeyAnimation GetKey(char letter)
    {
        var lowerLetter = char.ToLower(letter);
        var key = keys.FirstOrDefault(k => k.keyLower[0] == lowerLetter);
        if(key == null)
            return keys[0];
        return key;
    }
}

[Serializable] 
public class KeyAnimation
{
    [SpineAnimation]
    [SerializeField] public string key;
    [SerializeField] public string keyLower;
    [SerializeField] public float delayAnim;
}