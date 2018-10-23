using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;


namespace Cudafyed
{
    [Cudafy]
    public struct VectorStruct
    {
        public int x;
        public int y;
        public int z;
    }

    class Program
    {
        public const int N = 10000;


        static void Main(string[] args)
        {
            CudafyModes.Target = eGPUType.OpenCL;
            CudafyTranslator.Language = eLanguage.OpenCL;
            CudafyModule km = CudafyTranslator.Cudafy();
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);
            gpu.Launch().thekernel();
            Console.WriteLine(gpu.DeviceId);
            Console.WriteLine("Hello world");
            Console.ReadKey();
        }

        [Cudafy]
        public static void thekernel()
        {

        }
    }
}
