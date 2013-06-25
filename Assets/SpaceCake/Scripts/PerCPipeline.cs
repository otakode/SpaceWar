using UnityEngine;
using System.Collections;

public class PerCPipeline : MonoBehaviour
{
	static private PXCUPipeline pipeline = null;

	void Start()
	{
		if (PerCPipeline.pipeline == null)
		{
			PerCPipeline.pipeline = new PXCUPipeline();
			if (PerCPipeline.pipeline.Init(PXCUPipeline.Mode.VOICE_RECOGNITION|PXCUPipeline.Mode.GESTURE))
				Debug.Log("initialized Voice Recognition");
			else
				Debug.Log("initialize Voice Recognition FAILED");
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
}
