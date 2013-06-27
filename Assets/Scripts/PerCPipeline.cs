using UnityEngine;
using System.Collections;

public class PerCPipeline : MonoBehaviour
{
	static private PXCUPipeline pipeline = null;

	public struct PipelineData
	{
		public bool hasMainHand;
		public PXCMGesture.GeoNode mainHand;

		public bool hasSecondaryHand;
		public PXCMGesture.GeoNode secondaryHand;

		public bool hasMainGesture;
		public PXCMGesture.Gesture mainGesture;

		public bool hasSecondaryGesture;
		public PXCMGesture.Gesture secondaryGesture;

		public bool hasVoice;
		public PXCMVoiceRecognition.Recognition voice;
	}

	public delegate void PipelineUpdate(PipelineData data);
	public static PipelineUpdate pipelineUpdate;

	void Awake()
	{
		if (PerCPipeline.pipeline == null)
		{
			PerCPipeline.pipeline = new PXCUPipeline();
			if (PerCPipeline.pipeline.Init(/*PXCUPipeline.Mode.VOICE_RECOGNITION | */PXCUPipeline.Mode.GESTURE))
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

		PipelineData data = new PipelineData();
		data.hasMainHand = PerCPipeline.pipeline.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_HAND_MIDDLE, out data.mainHand);
		data.hasSecondaryHand = PerCPipeline.pipeline.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_HAND_MIDDLE, out data.secondaryHand);
		data.hasMainGesture = PerCPipeline.pipeline.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out data.mainGesture);
		data.hasSecondaryGesture = PerCPipeline.pipeline.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out data.secondaryGesture);
		//data.hasVoice = PerCPipeline.pipeline.QueryVoiceRecognized(out data.voice);

		PerCPipeline.pipeline.ReleaseFrame();

		PerCPipeline.pipelineUpdate(data);
	}
}
