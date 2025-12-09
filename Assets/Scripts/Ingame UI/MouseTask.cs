using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseTaskManager : MonoBehaviour
{
    private static MouseTaskManager _instance;

    private static MouseTask _currentTask = null;
    public static MouseTask CurrentTask
    {
        get => _currentTask;
        set
        {
            _currentTask = value;

            Texture2D texture = _currentTask == null || _currentTask.Sprite == null || _currentTask.Sprite.texture == null ? null : _currentTask.Sprite.texture;
            Debug.Log(texture);
            Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        }
    }
    public static void SetTask(UnitTask task)
        => CurrentTask = GetTask(task);
    private static MouseTask GetTask(UnitTask unitTask)
        => _instance.tasks.FirstOrDefault(x => x.UnitTask == unitTask);
    [SerializeField] List<MouseTask> tasks;

    private void Awake()
    {
        _instance = this;
    }

    [Serializable]
    public class MouseTask
    {
        public Sprite Sprite;
        public UnitTask UnitTask;
    }
}
