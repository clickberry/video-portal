using System.Collections.Generic;
using FfmpegBackend.Interface;


namespace FfmpegBackend
{
    public class EncodePipeline:IEncodePipeline
    {
        private readonly List<IPipelineStep> _list=new List<IPipelineStep>();

        public EncodePipeline()
        {
            
        }

        public EncodePipeline(IEncodeCreatorFactory factory)
        {
        }

        public void Start()
        {
            foreach (var pipelineStep in _list)
            {
                pipelineStep.Execute();
            }
        }

        public void Add(IPipelineStep pipelineStep)
        {
            _list.Add(pipelineStep);
        }
    }
}