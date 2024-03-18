using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISwiper : MonoBehaviour
{
    [SerializeField] private List<StepScroll> scrollSteps;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private RectMask2D rectMask2D;
    public Vector2 directionSwipe, ClickDownPosition, ClickUpPosition; 
    public bool canSwipe = false;
    public bool drag = false;
    public bool swipeProcess;
    Sequence sequence;
    public void Start()
    {
        directionSwipe = Vector2.zero;
        ClickDownPosition = Vector2.zero;
        ClickUpPosition = Vector2.zero;
        canSwipe = false;
        scrollSteps.ForEach(s => s.Init());
        MainContainer.instance.InputMessageSystem.Keyboard.onTapField += ScrollDeactive;
        MainContainer.instance.InputMessageSystem.Keyboard.onTapClose += ScrollActive;
    }

    private void ScrollDeactive()
    {
        scrollBar.image.enabled = false;
        scrollBar.handleRect.GetComponent<Image>().enabled = false;
        rectMask2D.enabled = false;
    }

    private void ScrollActive()
    {
        scrollBar.image.enabled = true;
        scrollBar.handleRect.GetComponent<Image>().enabled = true;
        rectMask2D.enabled = true;
    }

    private void Update()
    {
        if (canSwipe == false )
        {
            if (Input.GetMouseButton(0))
            {
                sequence?.Kill();
                ClickDownPosition = Input.mousePosition;
                drag = true;
                canSwipe = false;
                swipeProcess = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                ClickUpPosition = Input.mousePosition;
            }
            if ((ClickDownPosition != Vector2.zero || ClickUpPosition != Vector2.zero) && drag == true)
            {
                directionSwipe = ClickDownPosition - ClickUpPosition; 
               

                if ((directionSwipe.x > 0.1f || directionSwipe.x < 0.1f) && !Input.GetMouseButton(0))
                {
                    canSwipe = true;
                    drag = false;
                }

            }
           
        }
        if (canSwipe && drag == false && !Input.GetMouseButton(0) && swipeProcess == false)
        {
            swipeProcess = true;
            var step = scrollSteps.FirstOrDefault(s => s.IsCurrentStep(scrollRect.horizontalScrollbar.value));
            if(step != null)
            {
                scrollSteps.ForEach(s => s.myStep = false);
                step.myStep = true;

            }
            else
            {
                step = scrollSteps.FirstOrDefault(s => s.myStep);
            }
            var newStep = 0.1f;
           if( scrollRect.horizontalScrollbar.value >0.51)
            {
                newStep = 1;
            }
            else
            {
                newStep = 0;
            }
             sequence?.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(scrollRect.DOHorizontalNormalizedPos(newStep, 0.5f))
                    .AppendCallback(() =>
                {

                    canSwipe = false;
                    drag = false;
                    sequence?.Kill();

                });
        }
    }
}
[Serializable]
public class StepScroll
{
    
    public bool myStep;
    public float currentStep; 
    [SerializeField] private float requiredStep;
    [SerializeField] private float minimalStep;
    public void Init()
    {
        //if (currentStep != 0)
        //{
        //    float valStep = ((requiredStep - currentStep) / 2) - 0.01f;
        //    requiredStep = valStep + currentStep;
        //    minimalStep = valStep - currentStep;
        //    requiredStep = Mathf.Clamp01(requiredStep);
        //    minimalStep = Mathf.Clamp01(minimalStep);
        //}
        //else
        //{
        //    float valStep = 0.12f;
        //    requiredStep = valStep + currentStep;
        //}
    }
    public bool IsCurrentStep(float scrollBarStep)
    {
        if (scrollBarStep >= minimalStep && scrollBarStep <= requiredStep)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
