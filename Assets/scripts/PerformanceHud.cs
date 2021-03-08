using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class PerformanceHud : MonoBehaviour
{
    private float _cpuUsage = 0;
    private float _gpuUsage = 0;
    private float _gcTime = 0;
    private long _memory = 0;
    private float _fps;
    private int _systemMemory = 0;
    private float _renderTime = 0;
    private List<Statistic> _statistics;
    private TimeSpan _prevCPUPc;
    private TimeSpan _currCPUPc;
    private float _statsRefreshInterval = 0.5f;


    public GUIStyle DangerStyle;

    class Statistic
    {
        public string Name { get; set; }
        public Func<string> Value { get; set; }
        public Func<GUIStyle> Style { get; set; } = () => new GUIStyle();
    }

    void Start()
    {
        _statistics = new List<Statistic>();
        _statistics.Add(new Statistic { Name = "CPU Usage", Value = () => $"{_cpuUsage}%" });
        _statistics.Add(new Statistic { Name = "GPU Usage", Value = () => "" });
        _statistics.Add(new Statistic { Name = "GC Time (ms)", Value = () => "" });
        _statistics.Add(new Statistic { Name = "Render Time", Value = () => $"{_renderTime}ms" });
        _statistics.Add(new Statistic { Name = "Fps", Value = () => $"{_fps}" });
        _statistics.Add(new Statistic
        {
            Name = "Memory Usage",
            Value = () => $"{_memory}mb / {_systemMemory}mb",
            Style = () => _memory > 150 ? DangerStyle : new GUIStyle()
        });

        InvokeRepeating("GetProcessorUsage", _statsRefreshInterval, _statsRefreshInterval);
    }

    void Update()
    {
        _renderTime = Time.deltaTime * 1000;
        _memory = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        _systemMemory = SystemInfo.systemMemorySize;
        _fps = Time.frameCount / Time.time;
    }

    void OnGUI()
    {
        int yPos = 10;
        int lineHeight = 20;
        int boxWidth = 260;
        int padding = 20;

        GUI.Box(new Rect(10, yPos, boxWidth, _statistics.Count * lineHeight), "Statistics");
        foreach (var stat in _statistics)
        {
            yPos += 15;
            GUI.Label(new Rect(padding, yPos, boxWidth - padding * 2, lineHeight), $"{stat.Name}: {stat.Value()}", stat.Style());
        }
    }

    void GetProcessorUsage()
    {
        _prevCPUPc = _currCPUPc;
        _currCPUPc = new TimeSpan(0);
        Process[] allProcesses = Process.GetProcesses();
        foreach (var process in allProcesses)
            _currCPUPc += process.TotalProcessorTime;

        TimeSpan newCPUTime = _currCPUPc - _prevCPUPc;
        _cpuUsage = (int)((100 * newCPUTime.TotalSeconds / _statsRefreshInterval) / Environment.ProcessorCount);
    }
}

