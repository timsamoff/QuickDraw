using System.Collections;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("Serialized Sprite Positions")]
    [SerializeField] private Vector2 landscapeLeftStartPos;
    [SerializeField] private Vector2 landscapeRightStartPos;
    [SerializeField] private Vector2 portraitLeftStartPos;
    [SerializeField] private Vector2 portraitRightStartPos;

    [Header("Sprites (Prefabs)")]
    [SerializeField] private GameObject playerSidePrefab;
    [SerializeField] private GameObject aiSidePrefab;
    [SerializeField] private GameObject playerBackPrefab;
    [SerializeField] private GameObject aiFrontPrefab;

    private GameObject playerSide;
    private GameObject aiSide;
    private GameObject playerBack;
    private GameObject aiFront;

    private RectTransform leftRect;
    private RectTransform rightRect;
    private RectTransform backRect;
    private RectTransform frontRect;

    private void Start()
    {
        // Instantiate the sprite prefabs in the scene
        playerSide = Instantiate(playerSidePrefab, transform);
        aiSide = Instantiate(aiSidePrefab, transform);
        playerBack = Instantiate(playerBackPrefab, transform);
        aiFront = Instantiate(aiFrontPrefab, transform);

        // Get RectTransforms for positioning
        leftRect = playerSide.GetComponent<RectTransform>();
        rightRect = aiSide.GetComponent<RectTransform>();
        backRect = playerBack.GetComponent<RectTransform>();
        frontRect = aiFront.GetComponent<RectTransform>();

        UpdateSpritesForOrientation();
    }

    private void Update()
    {
        // Detect screen rotation and update sprite positions accordingly
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            // Landscape mode: Move sprites to their starting positions
            if (leftRect.anchoredPosition != landscapeLeftStartPos)
            {
                UpdateSpritesForOrientation();
            }
        }
        else if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            // Portrait mode: Transition sprites
            if (leftRect.anchoredPosition != portraitLeftStartPos)
            {
                StartCoroutine(TransitionSprites());
            }
        }
    }

    private void UpdateSpritesForOrientation()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            leftRect.anchoredPosition = landscapeLeftStartPos;
            rightRect.anchoredPosition = landscapeRightStartPos;
            backRect.anchoredPosition = portraitLeftStartPos; // In case you want to reset back sprite to portrait position for future transitions
            frontRect.anchoredPosition = portraitRightStartPos; // Same for the front sprite
        }
    }

    private IEnumerator TransitionSprites()
    {
        // Slide out the current sprites
        float transitionTime = 1f;
        float elapsedTime = 0f;

        Vector2 initialLeftPos = leftRect.anchoredPosition;
        Vector2 initialRightPos = rightRect.anchoredPosition;

        Vector2 initialBackPos = backRect.anchoredPosition;
        Vector2 initialFrontPos = frontRect.anchoredPosition;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / transitionTime;
            leftRect.anchoredPosition = Vector2.Lerp(initialLeftPos, new Vector2(-Screen.width, initialLeftPos.y), t);
            rightRect.anchoredPosition = Vector2.Lerp(initialRightPos, new Vector2(Screen.width, initialRightPos.y), t);

            backRect.anchoredPosition = Vector2.Lerp(initialBackPos, portraitLeftStartPos, t);
            frontRect.anchoredPosition = Vector2.Lerp(initialFrontPos, portraitRightStartPos, t);

            yield return null;
        }

        // Ensure final positions are set
        leftRect.anchoredPosition = new Vector2(-Screen.width, initialLeftPos.y);
        rightRect.anchoredPosition = new Vector2(Screen.width, initialRightPos.y);
        backRect.anchoredPosition = portraitLeftStartPos;
        frontRect.anchoredPosition = portraitRightStartPos;
    }
}