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
        var usedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        var totalMemory = SystemInfo.systemMemorySize;
        var fps = 1.0f / _deltaTime;

        // TODO: Tristan Trouver pourquoi une fois de temps en temps on buste de bcp le budget
        //UnityEngine.Debug.Assert(!_isLoaded || _renderTime < renderTimeBudget, $"Render time failed to meet expectations {_renderTime}/{renderTimeBudget}");

        cpuUsageTextElement.text = $"{_cpuUsage}%";
        renderTimeTextElement.text = $"{_renderTime}ms";
        fpsTextElement.text = $"{fps}";
        timePassedTextElement.text = $"{Time.time}s";
        memoryTextElement.text = $"{usedMemory}mb / {totalMemory}mb";

        if (usedMemory > memoryBudget)
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

