using UnityEngine;
using System.Collections;

public delegate void PipelineUpdate();

public class PerCPipeline : MonoBehaviour
{
	static private PXCUPipeline pipeline = null;
	public static PipelineUpdate pipelineUpdate;

	void Awake()
	{
		if (PerCPipeline.pipeline == null)
		{
			PerCPipeline.pipeline = new PXCUPipeline();
			if (PerCPipeline.pipeline.Init(PXCUPipeline.Mode.VOICE_RECOGNITION | PXCUPipeline.Mode.GESTURE))
				Debug.Log("Pipeline initialized");
			else
				Debug.Log("initialize Pipeline FAILED");
		}
	}
	
	void OnDisable()
	{
		PerCPipeline.pipeline.Close();
		PerCPipeline.pipeline.Dispose();
		PerCPipeline.pipeline = null;
	}

	static public PXCUPipeline GetPipeline()
	{
		return PerCPipeline.pipeline;
	}

	void Update()
	{
		if (PerCPipeline.pipeline == null || !PerCPipeline.pipeline.AcquireFrame(false))
			return;
		PerCPipeline.pipelineUpdate();
		PerCPipeline.pipeline.ReleaseFrame();
	}
}
