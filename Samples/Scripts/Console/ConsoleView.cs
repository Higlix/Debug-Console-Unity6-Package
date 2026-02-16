using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Reka.Samples.DebugConsole
{
	public sealed class ConsoleView : MonoBehaviour, IDragHandler, IPointerDownHandler
	{
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _enterButton;
		[SerializeField] private TMP_InputField _inputField;
		[SerializeField] private TMP_Text _consoleText;
		[SerializeField] private RectTransform _consoleViewRectTransform;

		void Awake()
		{
			try
			{
				if (_canvas == null)
				{
					throw new System.Exception("Canvas is not assigned");
				}
				if (_closeButton == null)
				{
					throw new System.Exception("CloseButton is not assigned");
				}
				if (_consoleViewRectTransform == null)
				{
					throw new System.Exception("ConsoleViewRectTransform is not assigned");
				}
				if (_inputField == null)
				{
					throw new System.Exception("InputField is not assigned");
				}
				if (_consoleText == null)
				{
					throw new System.Exception("ConsoleText is not assigned");
				}
				if (_enterButton == null)
				{
					throw new System.Exception("EnterButton is not assigned");
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
				Destroy(gameObject);
			}

			_inputField.interactable = true;
			_consoleText.text = "";
			_enterButton.onClick.AddListener(OnEnterButtonClick);
			Application.logMessageReceived += (condition, stackTrace, type) =>
			{
				AddConsoleText(condition, type);
			};
			_closeButton.onClick.AddListener(OnCloseButtonClick);
		}

		public void OnEnterButtonClick()
		{
			if (_inputField.text.Length > 0)
			{
				CallExecuteCommand(_inputField.text);
				_inputField.text = "";
			}
		}

		public void ClearConsoleText()
		{
			_consoleText.text = "";
		}

		public void OnCloseButtonClick()
		{
			Debug.Log("OnCloseButtonClick");
			_consoleViewRectTransform.gameObject.SetActive(false);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log("OnPointerDown");
			OnDrag(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			_inputField.ActivateInputField();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				_consoleViewRectTransform.parent as RectTransform,
				Mouse.current.position.ReadValue(),
				_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : GetComponentInParent<Canvas>().worldCamera,
				out Vector2 Q
			);

			RectTransform rt = _consoleViewRectTransform;
			Vector2 p = rt.pivot;
			Vector2 S_old = rt.sizeDelta;

			// 2. Current bottom-left in parent space (stays fixed)
			Vector2 C_min_old = rt.anchoredPosition - new Vector2(p.x * S_old.x, p.y * S_old.y);

			// 3. New size so that top-right is at Q
			float W_new = Q.x - C_min_old.x;
			float H_new = Q.y - C_min_old.y;
			float minWidth = 250f;
			float minHeight = 250f;
			W_new = Mathf.Max(minWidth, W_new);
			H_new = Mathf.Max(minHeight, H_new);

			// 4. New position so bottom-left stays at C_min_old
			Vector2 P_new = C_min_old + new Vector2(p.x * W_new, p.y * H_new);

			// 5. Apply
			rt.sizeDelta = new Vector2(W_new, H_new);
			rt.anchoredPosition = P_new;
			_consoleViewRectTransform = rt;
		}

		void Update()
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame &&
				_inputField.text.Length > 0)
			{
				_enterButton.onClick.Invoke();
			}
			else if (Keyboard.current.enterKey.wasPressedThisFrame)
			{
				_inputField.ActivateInputField();
			}
		}

		void CallExecuteCommand(string command)
		{
			if (Console.Instance != null)
			{
				Console.Instance.ExecuteCommand(command);
			}
			else
			{
				Debug.LogError("Console is not initialized");
			}
		}

		static string GetTimestamp()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		static string Colorize(string text, string colorHex)
		{
			return $"<color={colorHex}>[{GetTimestamp()}] {text}</color>\n";
		}

		/// <summary>Add a log message with color by type. Error=Red, Warning=Yellow, Log=White.</summary>
		public void AddConsoleText(string text, LogType type)
		{
			string color = type switch
			{
				LogType.Error => "#FF0000",
				LogType.Exception => "#FF0000",
				LogType.Assert => "#FFFFFF",
				LogType.Warning => "#FFFF00",
				_ => "#FFFFFF" // Log
			};
			_consoleText.text += Colorize(text, color);
			if (_consoleText.text.Length > 1000)
			{
				_consoleText.text = _consoleText.text.Substring(_consoleText.text.Length - 1000);
			}
		}
	}
}
