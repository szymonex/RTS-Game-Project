using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{

    public float cameraSpeed, zoomSpeed, groundHeight;
    public Vector2 cameraHeightMinMax;
    public Vector2 cameraRotationtMinMax;
    [Range(0, 1)]
    public float zoomLerp = 0.1f; //procentowa wartosc zooma
    [Range(0, 0.2f)]
    public float cursorTreshold;

    RectTransform selectionBox;
    new Camera camera;

    Vector2 mousePos, mousePosScreen, keyboardInput, mouseScroll;
    bool isCursorInGameScreen;

    private void Awake()
    {
        //keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        selectionBox = GetComponentInChildren<Image>(true).transform as RectTransform; //rzutowanie na nullowalny typ czyli wszystko co nie jest typem prymitywnym badz structem
        camera = GetComponent<Camera>();

    }

    private void Update()
    {
        UpdateMovement();
        UpdateZoom();
        UpdateClicks();

    }

    private void UpdateMovement() // tu definiujemy jak sie rusza kamera
    {
        keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mousePos = Input.mousePosition;
        //sprawdzamy czy kursor jest wewnatrz okna gry
        mousePosScreen = camera.ScreenToViewportPoint(mousePos);
        isCursorInGameScreen = mousePosScreen.x >= 0 && mousePosScreen.x <= 1 &&
            mousePosScreen.y >= 0 && mousePosScreen.y <= 1;

        Vector2 movementDirection = keyboardInput;
        if (isCursorInGameScreen)
        {
            if (mousePosScreen.x < cursorTreshold) movementDirection.x -= 1 - mousePosScreen.x / cursorTreshold;
            if (mousePosScreen.x > 1 - cursorTreshold) movementDirection.x += 1 - (1 - mousePosScreen.x) / cursorTreshold;
            if (mousePosScreen.y< cursorTreshold) movementDirection.y -= 1 - mousePosScreen.y / cursorTreshold;
            if (mousePosScreen.y> 1 - cursorTreshold) movementDirection.y+= 1 - (1 - mousePosScreen.y)/ cursorTreshold;
        }

        var deltaPosition = new Vector3(movementDirection.x, 0, movementDirection.y);
        deltaPosition *= cameraSpeed * Time.deltaTime; //wartość jak długo trwała ostatnbia klatka w sekundach, dzięki temu jesteśmy niezależni od fpsow i kamera zawsze bedzie przesuwala sie z ta sama szybkoscia w czasie rzeczywistym
        transform.localPosition += deltaPosition;

    }

    void UpdateZoom()
    {
        mouseScroll = Input.mouseScrollDelta;
        float zoomDelta = mouseScroll.y * zoomSpeed * Time.deltaTime;
        zoomLerp = Mathf.Clamp01(zoomLerp + zoomDelta); // funkcka ktora gdy jest wiecej niz jeden to i tak daje 1 a jak mniej niz 0 to daje 0

        //zachowanie kamery na zoom
        var position = transform.localPosition;
        position.y = Mathf.Lerp(cameraHeightMinMax.y, cameraHeightMinMax.x, zoomLerp) + groundHeight;
        transform.localPosition = position;

        var rotation = transform.localEulerAngles; //w ten sposob najlepiej pobierac rotacje
        rotation.x = Mathf.Lerp(cameraRotationtMinMax.y, cameraRotationtMinMax.x, zoomLerp);
        transform.localEulerAngles = rotation;
    }

    private void UpdateClicks()
    {
        //todo
        //selectionBox.anchoredPosition = mousePos;
    }

}
