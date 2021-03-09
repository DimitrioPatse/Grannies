using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] float stickMaxPixelDistance;
    [SerializeField] float maxJoystickToFingerPixelDistance;
    [SerializeField] Image bgImg;
    [SerializeField] Image stickImg;
    [SerializeField] float timeForMoveToFinger = 0.1f;
    [SerializeField] float repositionOffset = 50f;//Einai gia to repos oloklhroy toy joystick otan jefeygei to daxtulo
    [SerializeField] bool fixedJoystick;
    [SerializeField] bool fixedVelocity;
    Vector2 bgImgStartPos;
    Vector3 direction;
    public Vector3 Direction { get { return direction; } }

    public bool enableMove = true;

    void Start()
    {
        bgImgStartPos = bgImg.rectTransform.localPosition;
        fixedJoystick = (PlayerPrefsManager.GetFixedJoystick() == 1) ? true : false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enableMove)
        {
            direction = Vector3.zero;
            stickImg.rectTransform.anchoredPosition = Vector3.zero;
            return;
        }
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
            eventData.position, eventData.pressEventCamera, out Vector2 pos))
        {
            //For automatic resize per screen, it needs the exact value of the bgImage before setting the ayto resize. Here is 100
            pos.x = (pos.x / 100); // bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / 100); // bgImg.rectTransform.sizeDelta.y);

            //Adjust if pivot gives an offset
            Vector2 bgPivot = bgImg.rectTransform.pivot;
            pos.x += bgPivot.x - 0.5f;
            pos.y += bgPivot.y - 0.5f;

            float x = Mathf.Clamp(pos.x, -1, 1);
            float y = Mathf.Clamp(pos.y, -1, 1);
            direction = new Vector3(x, 0f, y);
            if (!fixedVelocity)
            {
                direction = (direction.magnitude > 1f) ? direction.normalized : direction;
            }
            else
            {
                direction.Normalize();
            }

            stickImg.rectTransform.anchoredPosition = new Vector2
                (direction.x * stickMaxPixelDistance, direction.z * stickMaxPixelDistance);
        }

        if (!fixedJoystick && Vector3.Distance(eventData.position, bgImg.rectTransform.position) > maxJoystickToFingerPixelDistance)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<RectTransform>(),
            eventData.position, eventData.pressEventCamera, out Vector2 poss))
            {
                LeanTween.cancel(bgImg.gameObject);
                Vector2 dis = new Vector2(direction.x, direction.z);
                LeanTween.move(bgImg.rectTransform, poss - (dis * (maxJoystickToFingerPixelDistance - repositionOffset)), timeForMoveToFinger);

            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        direction = Vector3.zero;
        stickImg.rectTransform.anchoredPosition = Vector3.zero;

        LeanTween.cancel(bgImg.gameObject);
        bgImg.rectTransform.anchoredPosition = bgImgStartPos;
       
        
        //HideJoystick();     TODO Na to balw se options epilogh an 8elei na kribetai to joystick
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        //ShowJoystick();     TODO Na to balw se options epilogh an 8elei na kribetai to joystick
        
        
        bgImg.rectTransform.position = eventData.position;
    }

    /// <summary>
    /// Deactivates the joystick images to hide the joystick UI from screen
    /// </summary>
    void HideJoystick()
    {
        bgImg.enabled = false;
        stickImg.enabled = false;
    }

    /// <summary>
    /// Displays the joystick UI on screen
    /// </summary>
    void ShowJoystick()
    {
        bgImg.enabled = true;
        stickImg.enabled = true;
    }
}
