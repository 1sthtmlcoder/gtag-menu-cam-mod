
using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[BepInPlugin("com.retromonke.vrmenumod", "VR Menu Mod", "1.0.0")]
public class VRMenuMod : BaseUnityPlugin
{
    private GameObject menu;
    private Camera cam;
    private bool freecam = false;
    private float camSpeed = 1f;
    private float fov = 60f;
    private bool followPlayer = false;
    private int spectateIndex = -1;
    private GameObject[] players;

    void Start()
    {
        cam = Camera.main;
        players = GameObject.FindGameObjectsWithTag("Player");
        CreateVRMenu();
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            menu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            menu.SetActive(false);
        }

        if (freecam)
        {
            float x = Input.GetAxis("Horizontal") * camSpeed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * camSpeed * Time.deltaTime;
            cam.transform.Translate(new Vector3(x, 0, z));
        }

        if (spectateIndex >= 0 && spectateIndex < players.Length && !followPlayer)
        {
            Transform t = players[spectateIndex].transform;
            cam.transform.position = t.position + new Vector3(0, 1, -2);
            cam.transform.LookAt(t);
        }

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                spectateIndex = (i == 0) ? 9 : i - 1;
            }
        }
    }

    void CreateVRMenu()
    {
        menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
        menu.transform.localScale = new Vector3(0.3f, 0.3f, 0.01f);
        menu.transform.SetParent(Camera.main.transform);
        menu.transform.localPosition = new Vector3(0.3f, -0.2f, 0.5f);

        CreateButton("Toggle Freecam", new Vector3(0, 0.15f, 0), () => freecam = !freecam);
        CreateButton("Follow Toggle", new Vector3(0, 0.05f, 0), () => followPlayer = !followPlayer);
        CreateButton("FOV +", new Vector3(0, -0.05f, 0), () => cam.fieldOfView += 5f);
        CreateButton("FOV -", new Vector3(0, -0.15f, 0), () => cam.fieldOfView -= 5f);
        CreateButton("Close", new Vector3(0, -0.25f, 0), () => menu.SetActive(false));
    }

    void CreateButton(string text, Vector3 offset, UnityEngine.Events.UnityAction action)
    {
        GameObject btn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        btn.transform.SetParent(menu.transform);
        btn.transform.localPosition = offset;
        btn.transform.localScale = new Vector3(0.25f, 0.05f, 0.01f);
        ButtonHandler bh = btn.AddComponent<ButtonHandler>();
        bh.OnClick = action;

        TextMesh tm = btn.AddComponent<TextMesh>();
        tm.text = text;
        tm.characterSize = 0.05f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
    }

    private class ButtonHandler : MonoBehaviour
    {
        public UnityEngine.Events.UnityAction OnClick;

        void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}
