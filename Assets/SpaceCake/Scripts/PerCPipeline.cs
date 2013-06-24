using UnityEngine;
using System.Collections;

public class PerCPipeline : MonoBehaviour
{
	static private PXCUPipeline pipeline;

	void Start()
	{
		if (PerCPipeline.pipeline == null)
		{
			PerCPipeline.pipeline = new PXCUPipeline();
			if (PerCPipeline.pipeline.Init(PXCUPipeline.Mode.VOICE_RECOGNITION))
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
		if (PerCPipeline.pipeline == null)
		{
			GameObject singleton = new GameObject("PerCPipeline");
			singleton.AddComponent<PerCPipeline>();
			for (int i = 0; i < 20; i++)
			{
				if (PerCPipeline.pipeline != null)
					break;
				System.Threading.Thread.Sleep(50);
			}
		}
		return PerCPipeline.pipeline;
	}
}
