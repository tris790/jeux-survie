using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class PerformanceHud : MonoBehaviour
{
    public Color warningColor;
    public Color bustedColor;

    public long memoryBudget;
    public float renderTimeBudget;
    public float timeToLoadGame;

    public Text cpuUsageTextElement;
    public Text renderTimeTextElement;
    public Text fpsTextElement;
    public Text timePassedTextElement;
    public Text memoryTextElement;

    private float _cpuUsage = 0;
    private float _renderTime = 0;
    private long _usedMemory = 0;
    private int _totalMemory = 0;
    private float _fps = 0;
    private float _deltaTime = 0;

    private TimeSpan _prevCPUPc;
    private TimeSpan _currCPUPc;
    private float _statsRefreshInterval = 0.5f;
    private bool _isLoaded = false;

    void Start()
    {
        InvokeRepeating(nameof(GetProcessorUsage), _statsRefreshInterval, _statsRefreshInterval);
        Invoke(nameof(UpdateIsLoaded), timeToLoadGame);
    }

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

        _renderTime = _deltaTime * 1000.0f;
        _usedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        _totalMemory = SystemInfo.systemMemorySize;
        _fps = 1.0f / _deltaTime;

        
    }

    void OnGUI()
    {
        cpuUsageTextElement.text = $"{_cpuUsage}%";
        renderTimeTextElement.text = $"{_renderTime}ms";
        fpsTextElement.text = $"{_fps}";
        timePassedTextElement.text = $"{Time.time}s";
        memoryTextElement.text = $"{_usedMemory}mb / {_totalMemory}mb";

        if (_usedMemory > memoryBudget)
            memoryTextElement.color = bustedColor;
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

    void UpdateIsLoaded()
    {
        _isLoaded = true;
    }
}

